using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Emit.Samples.Aop
{
    public static class TypeBuilderExtension
    {
        public static FieldBuilder DefineField(
            this TypeBuilder builder,
            ConstructorBuilder constructor,
            string name, 
            Type type,
            bool isEnd = false,
            FieldAttributes attributes = FieldAttributes.Private | FieldAttributes.InitOnly)
        {
            // 定义
            var fieldBuilder = builder.DefineField(name, type, attributes);

            // 初始化
            var il = constructor.GetILGenerator();// 获取构造函数ILGenerator
            il.Emit(OpCodes.Ldarg_0);// 将this压入计算推栈。
            il.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));// 新建一个对象并将其放入栈顶。
            il.Emit(OpCodes.Stfld, fieldBuilder);// 取出栈顶元素赋值给字段。

            if (isEnd)// 表示不在声明字段，构造函数可以返回了。
            {
                il.Emit(OpCodes.Ret);
            }

            return fieldBuilder;
        }

        public static PropertyBuilder DefineAutomaticProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            // 1. 定义属性的字段
            var filedBuilder = typeBuilder.DefineField($"_{propertyName.CamelCase()}", propertyType, FieldAttributes.Private);
            // 2. 定义属性
            var propBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, Type.EmptyTypes);
            // 3. 定义属性的get访问器
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                propertyType, Type.EmptyTypes);
            var getIl = getMethodBuilder.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);// 将this压入计算推栈。
            getIl.Emit(OpCodes.Ldfld, filedBuilder);// 将字段压入计算推栈。
            getIl.Emit(OpCodes.Ret);// Get方法返回

            propBuilder.SetGetMethod(getMethodBuilder);

            // 4. 定义属性的set访问器
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                null, new[] { propertyType });
            var setIl = setMethodBuilder.GetILGenerator();

            setIl.Emit(OpCodes.Ldarg_0);// 将this压入计算推栈。
            setIl.Emit(OpCodes.Ldarg_1);// 将赋值压入计算推栈。
            setIl.Emit(OpCodes.Stfld, filedBuilder);// 赋值给字段。
            setIl.Emit(OpCodes.Ret);// Set方法返回

            propBuilder.SetSetMethod(setMethodBuilder);

            return propBuilder;
        }
    }
}
