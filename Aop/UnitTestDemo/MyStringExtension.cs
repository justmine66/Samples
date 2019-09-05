using System.Linq;

namespace UnitTestDemo
{
    public class MyStringExtension
    {
        /// <summary>
        /// 创建一个反转字符串的方法，比如 输入hello，返回olleh
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Reverse(string str)
        {
            return new string(str.Reverse().ToArray());
        }
    }
}
