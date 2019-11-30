using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Emit.Samples
{
    public class FieldSample
    {
        // 使用Emit生成的最终结果类
        public class UserField
        {
            public static readonly string TokenPrefix = "Bearer";
            public UserField()
            {
                id = Guid.NewGuid().ToString("N");
            }

            public readonly string id;

            public string userName;

            private string passwordHash = "123456";

            public string GetPasswordHash()
            {
                return passwordHash;
            }

            public void SetPasswordHash(string password)
            {
                passwordHash = password;
            }
        }

        public static void Run()
        {
            // 1. 创建类
            var name = "Emit.Samples.Filed";
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule(name);
            var typeBuilder = moduleBuilder.DefineType("UserField", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit);

            // 2. 创建静态字段
            var tokenPrefixBuilder = typeBuilder.DefineField("TokenPrefix", typeof(string), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly);

            // 3. 创建非静态字段
            var idBuilder = typeBuilder.DefineField("id", typeof(string), FieldAttributes.Public | FieldAttributes.InitOnly);
            var userNameBuilder = typeBuilder.DefineField("userName", typeof(string), FieldAttributes.Public);
            var passwordHashBuilder = typeBuilder.DefineField("passwordHash", typeof(string), FieldAttributes.Private);

            // 4. 字段操作
            // 在字段后面写默认值的方法是C#的语法糖，实际是在构造器中进行初始化，静态字段在静态构造器中赋值，对象字段在构造器中赋值。

            // 4.1 初始化静态字段
            var staticCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig, CallingConventions.Standard, Type.EmptyTypes);
            var staticCtorIL = staticCtorBuilder.GetILGenerator();
            // 将常量字符串"Bearer"放入栈顶
            staticCtorIL.Emit(OpCodes.Ldstr, "Bearer");
            // 取出栈顶元素赋值给字段TokenPrefix
            staticCtorIL.Emit(OpCodes.Stfld, tokenPrefixBuilder);
            staticCtorIL.Emit(OpCodes.Ret);

            // 4.2 初始化实例字段
            var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, CallingConventions.HasThis, Type.EmptyTypes);
            var ctorIl = ctorBuilder.GetILGenerator();
            //将this压入栈中
            ctorIl.Emit(OpCodes.Ldarg_0);
            //将常量字符串"123456"放入栈顶
            ctorIl.Emit(OpCodes.Ldstr, "123456");
            //取出栈顶元素赋值给字段
            ctorIl.Emit(OpCodes.Stfld, passwordHashBuilder);
            //返回
            ctorIl.Emit(OpCodes.Ret);

            // 5. 编写方法
            var getPasswordHashMethodBuilder = typeBuilder.DefineMethod("GetPasswordHash", MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, typeof(string), Type.EmptyTypes);
            var getPasswordHashIl = getPasswordHashMethodBuilder.GetILGenerator();
            //将this压入栈中
            getPasswordHashIl.Emit(OpCodes.Ldarg_0);
            //将字段值压入到栈中
            getPasswordHashIl.Emit(OpCodes.Ldfld, passwordHashBuilder);
            //返回
            getPasswordHashIl.Emit(OpCodes.Ret);

            //var setPasswordHashMethodBuilder = typeBuilder.DefineMethod("SetPasswordHash",
            //    MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.HasThis, null,
            //    new[] {typeof(string)});
            //var setPasswordHashIl = setPasswordHashMethodBuilder.GetILGenerator();
            //setPasswordHashIl.Emit(OpCodes.Ldarg_0);
            //setPasswordHashIl.Emit(OpCodes.Ldstr, "这是静态赋值");
            //setPasswordHashIl.Emit(OpCodes.Ldarg_1);
            //setPasswordHashIl.Emit(OpCodes.Ldfld, passwordHashBuilder);
            //setPasswordHashIl.Emit(OpCodes.Ret);

            // 6. 创建对象
            var type = typeBuilder.CreateType();
            dynamic user = Activator.CreateInstance(type);

            // 7. 调用方法查看结果
            Console.WriteLine("初始值: "+user.GetPasswordHash());
            user.SetPasswordHash();
            Console.WriteLine("赋值: " + user.GetPasswordHash());
        }
    }
}
