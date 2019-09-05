using System;
using Castle.DynamicProxy;

namespace Aop.Demos.CrossCuttings
{
    public class BeforeAndAfterAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Before");
            invocation.Proceed();
            Console.WriteLine("Before");
        }
    }
}
