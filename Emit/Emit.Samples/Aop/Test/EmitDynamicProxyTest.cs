using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Emit.Samples.Aop.Test
{
    public class EmitDynamicProxyTest
    {
        public static void FaultTolerantOfRealize()
        {
            var instance = DynamicProxy.CreateProxy<IBusinessClass, BusinessClass>();
            //IBusinessClass Instance = new BusinessClassProxy();

            instance.Test();
            instance.GetInt(123);
            instance.NoArgument();
            instance.ThrowException();
            instance.ArgumentVoid(123, "123");
            instance.GetBool(false);
            instance.GetString("123");
            instance.GetFloat(123f);
            instance.GetDouble(123.123);
            instance.GetObject(null);
            instance.GetOperateResult(123, "123");
            instance.GetOperateResults(new List<OperateResult>());
            instance.GetDecimal(123.123m);
            instance.GetDateTime(DateTime.Now);
        }

        public static void FaultTolerantOfInherit()
        {
            //IBusinessClass Instance = new BusinessClassVirtualProxy();
            var instance = DynamicProxy.CreateProxy<BusinessClassInherit>();

            instance.Test();
            instance.NoArgument();
            instance.GetBool(false);
            instance.GetInt(123);
            instance.GetFloat(123f);
            instance.GetDouble(123.123);
            instance.GetString("123");
            instance.GetObject(null);
            instance.GetOperateResult(123, "123");
            instance.GetOperateResults(new List<OperateResult>());
            instance.GetDecimal(123.123m);
            instance.GetDateTime(DateTime.Now);
        }

        public static void Performance2()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 1000000; i++)
            {
                var instance = BusinessClass.Instance2;
                instance.Test();
                var result = instance.GetOperateResult(111, "333");
                var intResult = instance.GetInt(222);
            }

            stopwatch.Stop();
            Console.WriteLine($"直接调用耗时:{stopwatch.ElapsedMilliseconds}");
            //不使用代理类，百万次调用  58 ms
        }

        public static void Performance1()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 1000000; i++)
            {
                var instance = BusinessClass.Instance;
                instance.Test();
                var result = instance.GetOperateResult(111, "333");
                var intResult = instance.GetInt(222);
            }

            stopwatch.Stop();
            Console.WriteLine($"动态代理调用耗时:{stopwatch.ElapsedMilliseconds}");
            //使用了代理类，但是没有添加任何类标签和方法标签，百万次调用 73ms    （相当于直接调用）
            //使用了代理类，添加类代理标签，百万次调用  1112ms  （Invoke）
            //使用了代理类，添加了方法代理标签，百万次调用   177ms    (拆装箱）
            //使用了代理类，添加了类代理标签和方法代理标签，百万次调用  1231ms  （Invoke+拆装箱）
        }

        public static void Performance3()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 1000000; i++)
            {
                var instance = BusinessClassInherit.Instance;
                instance.Test();
                var result = instance.GetOperateResult(111, "333");
                var intResult = instance.GetInt(222);
            }

            stopwatch.Stop();
            Console.WriteLine($"动态代理调用耗时:{stopwatch.ElapsedMilliseconds}");
            //使用了代理类，但是没有添加任何类标签和方法标签，百万次调用 73ms    （相当于直接调用）
            //使用了代理类，添加类代理标签，百万次调用  1074ms  （Invoke）
            //使用了代理类，添加了方法代理标签，百万次调用   191ms    (拆装箱）
            //使用了代理类，添加了类代理标签和方法代理标签，百万次调用  1216ms  （Invoke+拆装箱）
        }

        public static void ExceptionFilter()
        {
            var instance = DynamicProxy.CreateProxy<IBusinessClass, BusinessClass>();
            instance.ThrowException();
        }
    }
}
