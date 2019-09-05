using System;

namespace Aop.demo.AspnetCore.Attritbutes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValueAttribute : Attribute
    {
        public ValueAttribute(string value = "")
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}
