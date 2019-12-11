using System;
using System.Diagnostics;

namespace Emit.Samples.Aop.Test
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptorAttribute : InterceptorBaseAttribute
    {
        public override object Invoke(object @object, string method, object[] parameters)
        {
            Console.WriteLine($"interceptor does something before invoke [{@method}]...");

            object obj = null;

            try
            {
                obj = base.Invoke(@object, method, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine($"interceptor does something after invoke [{@method}]...");

            return obj;
        }
    }
}
