using System;
using System.Diagnostics;

namespace Emit.Samples.Aop.Test
{
    public class ActionAttribute : ActionBaseAttribute
    {
        public override void Before(string @method, object[] parameters)
        {
            Console.WriteLine($"Action Before  1111111 ,method:{method},parameters:{parameters}");
        }

        public override object After(string method, object result)
        {
            Console.WriteLine($"Action After  1111111 ,method:{method},parameters:{result}");
            return base.After(method, result);
        }
    }

    public class Action2Attribute : ActionBaseAttribute
    {
        public override void Before(string @method, object[] parameters)
        {
            Console.WriteLine($"Action Before  2222222 ,method:{method},parameters:{parameters}");
        }

        public override object After(string method, object result)
        {
            Console.WriteLine($"Action After  2222222 ,method:{method},parameters:{result}");
            return base.After(method, result);
        }
    }

    public class Action3Attribute : ActionBaseAttribute
    {
        public override void Before(string @method, object[] parameters)
        {
            Console.WriteLine($"Action Before  3333333 ,method:{method},parameters:{parameters}");
        }

        public override object After(string method, object result)
        {
            Console.WriteLine($"Action After  3333333 ,method:{method},parameters:{result}");
            return base.After(method, result);
        }
    }
}
