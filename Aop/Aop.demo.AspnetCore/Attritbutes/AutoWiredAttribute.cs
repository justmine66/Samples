using System;

namespace Aop.demo.AspnetCore.Attritbutes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoWiredAttribute : Attribute
    {
    }
}
