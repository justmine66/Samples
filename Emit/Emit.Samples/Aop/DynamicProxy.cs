using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Emit.Samples.Aop
{
    public class DynamicProxy
    {
        private static readonly string[] IgnoreMethodName = new[] { "GetType", "ToString", "GetHashCode", "Equals" };

        public static TInterface CreateProxy<TInterface, TImp>()
            where TImp : class, new()
            where TInterface : class
            => Invoke<TInterface, TImp>();

        public static TProxyClass CreateProxy<TProxyClass>()
            where TProxyClass : class, new()
            => Invoke<TProxyClass, TProxyClass>(true);

        private static TInterface Invoke<TInterface, TImp>(bool inheritMode = false)
            where TImp : class
            where TInterface : class
            => inheritMode
            ? CreateProxy(typeof(TImp)) as TInterface
            : CreateProxy(typeof(TInterface), typeof(TImp)) as TInterface;

        public static object CreateProxy(Type interfaceType, Type impType)
        {
            var nameOfAssembly = impType.Name + "ProxyAssembly";
            var nameOfModule = impType.Name + "ProxyModule";
            var nameOfType = impType.Name + "Proxy";

            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(nameOfAssembly), AssemblyBuilderAccess.Run);
            var moduleBuilder = assembly.DefineDynamicModule(nameOfModule);
            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public, null, new[] { interfaceType });
            var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;

            return Invoke(impType, typeBuilder, methodAttributes);
        }

        public static object CreateProxy(Type impType)
        {
            var nameOfAssembly = impType.Name + "ProxyAssembly";
            var nameOfModule = impType.Name + "ProxyModule";
            var nameOfType = impType.Name + "Proxy";

            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(nameOfAssembly), AssemblyBuilderAccess.Run);
            var moduleBuilder = assembly.DefineDynamicModule(nameOfModule);
            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public, impType);
            var methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;

            return Invoke(impType, typeBuilder, methodAttributes);
        }

        private static object Invoke(Type impType, TypeBuilder typeBuilder, MethodAttributes methodAttributes)
        {
            var interceptorType = impType.GetCustomAttribute(typeof(InterceptorBaseAttribute))?.GetType();

            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

            // ---- define fields ----
            FieldBuilder interceptor = null;
            if (interceptorType != null)
                interceptor = typeBuilder.DefineField(constructorBuilder, "_interceptor", interceptorType);

            var implementation = typeBuilder.DefineField(constructorBuilder, "_implementation", impType, true);

            // ---- define methods ----
            var methodsOfType = impType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methodsOfType)
            {
                // ignore method
                if (IgnoreMethodName.Contains(method.Name))
                    continue;

                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var methodBuilder = typeBuilder.DefineMethod(method.Name, methodAttributes, CallingConventions.Standard, method.ReturnType, methodParameterTypes);
                var ilMethod = methodBuilder.GetILGenerator();

                // set local field
                var methodName = ilMethod.DeclareLocal(typeof(string));     //instance of method name
                var parameters = ilMethod.DeclareLocal(typeof(object[]));   //instance of parameters
                var result = ilMethod.DeclareLocal(typeof(object));         //instance of result
                var actionTypeBuilders = new Dictionary<Type, LocalBuilder>();

                // attribute init
                // method can override class attribute
                if (method.GetCustomAttributes<ActionBaseAttribute>().Any())
                {
                    foreach (var attr in method.GetCustomAttributes<ActionBaseAttribute>())
                        actionTypeBuilders.AddOrUpdate(attr.GetType());
                }
                else if (impType.GetCustomAttributes<ActionBaseAttribute>().Any())
                {
                    foreach (var attr in impType.GetCustomAttributes<ActionBaseAttribute>())
                        actionTypeBuilders.AddOrUpdate(attr.GetType());
                }

                foreach (var item in actionTypeBuilders.Select(t => t.Key).ToArray())
                {
                    var actionAttributeObj = ilMethod.DeclareLocal(item);
                    ilMethod.Emit(OpCodes.Newobj, item.GetConstructor(Type.EmptyTypes));
                    ilMethod.Emit(OpCodes.Stloc, actionAttributeObj);
                    actionTypeBuilders[item] = actionAttributeObj;
                }

                // if no attribute
                if (interceptor != null || actionTypeBuilders.Any())
                {
                    ilMethod.Emit(OpCodes.Ldstr, method.Name);
                    ilMethod.Emit(OpCodes.Stloc, methodName);

                    ilMethod.Emit(OpCodes.Ldc_I4, methodParameterTypes.Length);
                    ilMethod.Emit(OpCodes.Newarr, typeof(object));
                    ilMethod.Emit(OpCodes.Stloc, parameters);

                    // build the method parameters
                    for (var j = 0; j < methodParameterTypes.Length; j++)
                    {
                        ilMethod.Emit(OpCodes.Ldloc, parameters);
                        ilMethod.Emit(OpCodes.Ldc_I4, j);
                        ilMethod.Emit(OpCodes.Ldarg, j + 1);
                        //box
                        ilMethod.Emit(OpCodes.Box, methodParameterTypes[j]);
                        ilMethod.Emit(OpCodes.Stelem_Ref);
                    }
                }

                // dynamic proxy action before
                if (actionTypeBuilders.Any())
                {
                    // load arguments
                    foreach (var item in actionTypeBuilders)
                    {
                        ilMethod.Emit(OpCodes.Ldloc, item.Value);
                        ilMethod.Emit(OpCodes.Ldloc, methodName);
                        ilMethod.Emit(OpCodes.Ldloc, parameters);
                        ilMethod.Emit(OpCodes.Call, item.Key.GetMethod("Before"));
                    }
                }

                if (interceptorType != null)
                {
                    // load arguments
                    ilMethod.Emit(OpCodes.Ldarg_0);//this
                    ilMethod.Emit(OpCodes.Ldfld, interceptor);
                    ilMethod.Emit(OpCodes.Ldarg_0);//this
                    ilMethod.Emit(OpCodes.Ldfld, implementation);
                    ilMethod.Emit(OpCodes.Ldloc, methodName);
                    ilMethod.Emit(OpCodes.Ldloc, parameters);
                    // call Invoke() method of Interceptor
                    ilMethod.Emit(OpCodes.Callvirt, interceptorType.GetMethod("Invoke"));
                }
                else
                {
                    // direct call method
                    if (method.ReturnType == typeof(void) && !actionTypeBuilders.Any())
                    {
                        ilMethod.Emit(OpCodes.Ldnull);
                    }

                    ilMethod.Emit(OpCodes.Ldarg_0);//this
                    ilMethod.Emit(OpCodes.Ldfld, implementation);
                    for (var j = 0; j < methodParameterTypes.Length; j++)
                    {
                        ilMethod.Emit(OpCodes.Ldarg, j + 1);
                    }
                    ilMethod.Emit(OpCodes.Callvirt, impType.GetMethod(method.Name));
                    // box
                    if (actionTypeBuilders.Any())
                    {
                        if (method.ReturnType != typeof(void))
                            ilMethod.Emit(OpCodes.Box, method.ReturnType);
                        else
                            ilMethod.Emit(OpCodes.Ldnull);
                    }
                }

                // dynamic proxy action after
                if (actionTypeBuilders.Any())
                {
                    ilMethod.Emit(OpCodes.Stloc, result);

                    // 1->2 before and 2->1 after
                    foreach (var item in actionTypeBuilders.Reverse())
                    {
                        ilMethod.Emit(OpCodes.Ldloc, item.Value);
                        ilMethod.Emit(OpCodes.Ldloc, methodName);
                        ilMethod.Emit(OpCodes.Ldloc, result);
                        ilMethod.Emit(OpCodes.Callvirt, item.Key.GetMethod("After"));

                        //if no void return,set result
                        if (method.ReturnType == typeof(void))
                            ilMethod.Emit(OpCodes.Pop);
                        else
                            ilMethod.Emit(OpCodes.Stloc, result);
                    }
                }
                else
                {
                    // if no void return,set result
                    if (method.ReturnType == typeof(void))
                        ilMethod.Emit(OpCodes.Nop);
                    else
                        ilMethod.Emit(OpCodes.Stloc, result);
                }

                // pop the stack if return void
                if (method.ReturnType == typeof(void))
                {
                    // if no action attribute，void method need pop(action attribute method has done before)
                    if (!actionTypeBuilders.Any())
                        ilMethod.Emit(OpCodes.Pop);
                }
                else
                {
                    // unbox, if direct invoke, no box
                    if (interceptor != null || actionTypeBuilders.Any())
                    {
                        ilMethod.Emit(OpCodes.Ldloc, result);
                        ilMethod.Emit(method.ReturnType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, method.ReturnType);
                    }
                }

                // complete
                ilMethod.Emit(OpCodes.Ret);
            }

            var type = typeBuilder.CreateType();

            return Activator.CreateInstance(type);
        }
    }
}
