using System;

namespace Aop.demo.AspnetCore.Attritbutes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : Attribute
    {
    }
}
