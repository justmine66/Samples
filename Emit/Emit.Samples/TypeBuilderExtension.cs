using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Emit.Samples
{
    public static class TypeBuilderExtension
    {
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
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, filedBuilder);
            getIl.Emit(OpCodes.Ret);

            propBuilder.SetGetMethod(getMethodBuilder);

            // 4. 定义属性的set访问器
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName,
                null, new[] {propertyType});
            var setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, filedBuilder);
            setIl.Emit(OpCodes.Ret);
            
            propBuilder.SetSetMethod(setMethodBuilder);

            return propBuilder;
        }
    }
}
