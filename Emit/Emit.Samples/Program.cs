using System;
using Emit.Samples.Aop.Test;

namespace Emit.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            //FieldSample.Run();
            //Console.WriteLine("-------------");
            //PropertySample.Run();
            //Console.WriteLine("-------------");

            //AddSample.Run();
            EmitDynamicProxyTest.FaultTolerantOfRealize();
            Console.WriteLine("-------------");
            EmitDynamicProxyTest.FaultTolerantOfInherit();
            Console.WriteLine("-------------");
            EmitDynamicProxyTest.Performance1();

            Console.Read();
        }
    }
}
