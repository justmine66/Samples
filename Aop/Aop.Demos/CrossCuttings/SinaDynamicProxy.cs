using System;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.AssemblyBuilder;

namespace Aop.Demos.CrossCuttings
{
    public static class SinaDynamicProxy
    {
        public static ISinaService CreateProxy()
        {
            //生成一个动态代理类型并返回
            var type = CreateDynamicProxyType();
            //使用Activator和上面的动态代理类型实例化它的一个对象
            var proxy = Activator.CreateInstance(type, new object[] { new MySinaService() }) as ISinaService;
            return proxy;
        }

        /// <summary>
        /// 使用Reflection.Emit生产动态代理，如下：
        /// public class MySinaServiceProxy : ISinaService
        /// {
        ///    private MySinaService _service;
        ///    public MySinaServiceProxy(MySinaService service)
        ///    {
        ///        _service = service;
        ///    }
        ///    public void SendMsg(string msg)
        ///    {
        ///        Console.WriteLine("Before");
        ///        _service.SendMsg(msg);
        ///        Console.WriteLine("After");
        ///    }
        /// }
        /// </summary>
        /// <returns></returns>
        private static Type CreateDynamicProxyType()
        {
            //所有的Reflection.Emit方法都在这里
            //1. 定义AssemblyName
            var assemblyName = new AssemblyName("MyProxies");
            //2. 创建ModuleBuilder
            var moduleBuilder = DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule("MyProxies");

            var typeBuilder = moduleBuilder.DefineType(
                "MySinaServiceProxy",//要创建的类型的名称
                TypeAttributes.Public | TypeAttributes.Class,//类型的特性
                typeof(object),//基类
                new[] { typeof(ISinaService) }//实现的接口
                );

            var fieldBuilder = typeBuilder.DefineField(
                "_realObject",
                typeof(MySinaService),
                FieldAttributes.Private
                );

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.HasThis,
                new[] { typeof(MySinaService) }
                );

            var ilGenerator = constructorBuilder.GetILGenerator();

            //3.获取一个ILGenerator将代码添加到构造函数
            //将this加载到计算栈
            ilGenerator.Emit(OpCodes.Ldarg_0);
            //将构造函数的形参加载到栈
            ilGenerator.Emit(OpCodes.Ldarg_1);
            //将计算结果保存到字段
            ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            //从构造函数返回
            ilGenerator.Emit(OpCodes.Ret);

            var methodBuilder = typeBuilder.DefineMethod(
                "SendMsg",//方法名称
                MethodAttributes.Public | MethodAttributes.Virtual,//方法修饰符
                typeof(void),//无返回值
                new[] { typeof(string) }//有个字符串参数
                );
            //指定要构建的方法实现了ISinaService接口的SendMsg方法
            typeBuilder.DefineMethodOverride(
                methodBuilder,
                typeof(ISinaService).GetMethod("SendMsg")
                );

            //获取一个ILGenerator将代码添加到SendMsg方法
            var sendMsgIlGenerator = methodBuilder.GetILGenerator();
            //加载字符串变量到计算栈
            sendMsgIlGenerator.Emit(OpCodes.Ldstr, "Before");
            //调用Console类的静态WriteLine方法
            sendMsgIlGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }));
            //将参数argument0（this）加载到栈
            sendMsgIlGenerator.Emit(OpCodes.Ldarg_0);
            //将字段_realObject加载到栈
            sendMsgIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            //加载SendMsg的参数到栈
            sendMsgIlGenerator.Emit(OpCodes.Ldarg_1);
            //调用字段上的SendMsg方法
            sendMsgIlGenerator.Emit(OpCodes.Call, fieldBuilder.FieldType.GetMethod("SendMsg"));
            //加载字符串After到栈
            sendMsgIlGenerator.Emit(OpCodes.Ldstr, "After");
            //调用Console类的静态WriteLine方法
            sendMsgIlGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }));
            //返回
            sendMsgIlGenerator.Emit(OpCodes.Ret);

            return typeBuilder.CreateType();
        }
    }
}
