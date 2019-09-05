using System;
using System.Reflection;
using Aop.demo.AspnetCore.Attritbutes;
using Castle.DynamicProxy;

namespace Aop.demo.AspnetCore.CrossCuttings
{
    /// <summary>
    /// 事物拦截器
    /// </summary>
    public class TransactionalInterceptor : StandardInterceptor
    {
        private IUnitOfWork Uow { set; get; }

        public TransactionalInterceptor(IUnitOfWork uow)
        {
            Uow = uow;
        }

        protected override void PreProceed(IInvocation invocation)
        {
            Console.WriteLine("{0}拦截前", invocation.Method.Name);

            var method = invocation.TargetType.GetMethod(invocation.Method.Name);
            if (method != null && method.GetCustomAttribute<TransactionalAttribute>() != null)
            {
                Uow.BeginTransaction();
            }
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            invocation.Proceed();
        }

        protected override void PostProceed(IInvocation invocation)
        {
            Console.WriteLine("{0}拦截后， 返回值是{1}", invocation.Method.Name, invocation.ReturnValue);

            var method = invocation.TargetType.GetMethod(invocation.Method.Name);
            if (method != null && method.GetCustomAttribute<TransactionalAttribute>() != null)
            {
                Uow.Commit();
            }
        }
    }
}
