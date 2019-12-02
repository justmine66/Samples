using System;

namespace Emit.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            FieldSample.Run();
            Console.WriteLine("-------------");
            PropertySample.Run();

            Console.Read();
        }
    }
}
