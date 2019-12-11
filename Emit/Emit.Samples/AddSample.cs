using System;
using System.Reflection.Emit;

namespace Emit.Samples
{
    public static class AddSample
    {
        // 使用IL实现如下方法：
        //static void Main()
        //{
        //    int i = 1;
        //    int j = 2;
        //    int k = 3;

        //    Console.WriteLine(i + j + k);
        //}

        public static void Run()
        {
            var method = new DynamicMethod("Main", null, Type.EmptyTypes);
            var il = method.GetILGenerator();

            // 一个Evaluation Stack的生存周期跟一个函数是一样的，通过函数最后的ret指令返回并传递返回值：https://msdn.microsoft.com/library/system.reflection.emit.opcodes.ret.aspx , 并不是全局的。
            // Call Stack为Thread Stack，是系统为一个线程分配的，通常是1M，全局的。

            //IL_0001: ldc.i4.1    //加载第一个变量i  
            //首先对 ldc.i4.1 做下细解：变量的值为1 时IL指令就是ldc.i4.1 ,变量值为2 时IL指令就是ldc.i4.2，依此类推一直到ldc.i4.8
            //当为-1时IL指令为ldc.i4.M1, 当超过8时就是一个统一指令ldc.i4.S

            //1 ldc.i4.1 把值取出来后先存在Evaluation Stack中, 执行了stloc.0后才会存入Call Stack的Record Frame中.
            //2 ldloc.0 把取出来后也是先压入Evaluation Stack 等持指令
            //3 add 操作完成后值是暂存于 Evaluation Stack中的

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldc_I4_1); // 加载第一个变量"i"的值1（压入Evaluation Stack中）
            il.Emit(OpCodes.Stloc_0); // 从栈中把"i"的值弹出并赋值给Call Stack的Record Frame第0个位置
            il.Emit(OpCodes.Ldc_I4_2); // 加载第一个变量"j"的值2（压入Evaluation Stack中）
            il.Emit(OpCodes.Stloc_1); // 从栈中把"j"的值弹出并赋值给Call Stack的Record Frame第1个位置
            il.Emit(OpCodes.Ldc_I4_3); // 加载第一个变量"k"的值3（压入Evaluation Stack中）
            il.Emit(OpCodes.Stloc_2); // 从栈中把"k"的值弹出并赋值给Call Stack的Record Frame第2个位置

            // 上面代码初始化完成后要开始输出了，所以要把数据从Call Stack的Record Frame取出.
            il.Emit(OpCodes.Ldloc_0); //取Call Stack的Record Frame中位置为0的元素的值("i"的值)并压入Evaluation Stack栈中. 
            il.Emit(OpCodes.Ldloc_1); //取Call Stack的Record Frame中位置为1的元素的值("j"的值)并压入Evaluation Stack栈中. 
            il.Emit(OpCodes.Add); // 做加法操作
            il.Emit(OpCodes.Ldloc_2); //取Call Stack的Record Frame中位置为2的元素的值("k"的值)并压入Evaluation Stack栈中. 
            il.Emit(OpCodes.Add); // 做加法操作
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);

            // 创建委托
            var main = method.CreateDelegate(typeof(Action)) as Action;
        }
    }
}
