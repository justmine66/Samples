using System;
using Aop.demo.AspnetCore.Attritbutes;

namespace Aop.demo.AspnetCore.Domain
{
    /// <summary>
    /// 汽车引擎
    /// </summary>
    public class Engine
    {
        [Value("HelpNumber")]
        public string HelpNumber { set; get; }

        public virtual void Start()
        {
            Console.WriteLine("发动机启动");
            Stop();
        }

        public virtual void Stop()
        {
            Console.WriteLine("发动机熄火,拨打求救电话" + HelpNumber);
        }
    }

    public interface ICar
    {
        Engine Engine { set; get; }

        void Fire();
    }

    public class Car : ICar
    {
        [AutoWired]
        public Engine Engine { set; get; }

        [Value("oilNo")]
        public int OilNo { set; get; }

        [Transactional]
        public void Fire()
        {
            Console.WriteLine("加满" + OilNo + "号汽油,点火");
            Engine.Start();
        }
    }
}
