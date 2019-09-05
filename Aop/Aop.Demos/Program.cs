using System;
using Aop.Demos.CrossCuttings;

namespace Aop.Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. 动态代理
            //var proxy = SinaDynamicProxy.CreateProxy();
            //proxy.SendMsg("消息");

            // 2. 静态编织
            var my = new MyClass();
            my.MyMethod();

            Console.Read();
        }
    }
}
