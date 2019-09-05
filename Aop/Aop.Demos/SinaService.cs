using System;

namespace Aop.Demos
{
    public interface ISinaService
    {
        void SendMsg(string msg);
    }

    public class MySinaService : ISinaService
    {
        public void SendMsg(string msg)
        {
            Console.WriteLine($"[{msg}] has been sent!");
        }
    }
}
