using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanTest
{
    public class MemoryLayoutSample
    {
        public static void Test()
        {
            // x86: 4 bytes; x64: 8 bytes;
            var pointerSize = IntPtr.Size;
            var size1 = Marshal.SizeOf<ValueType>();
            var size2 = Unsafe.SizeOf<ValueType>();
            var size3 = Unsafe.SizeOf<ReferenceType>();

            Console.WriteLine($"Pointer Size: {pointerSize}");
            Console.WriteLine($"Marshal.SizeOf<ValueType>(): {size1}");
            Console.WriteLine($"Unsafe.SizeOf<ValueType>(): {size2}");
            Console.WriteLine($"Marshal.SizeOf<ReferenceType>(): {size3}");
        }
    }

    // Value Types don’t have any additional overhead members. 
    // 12 bytes
    public struct ValueType
    {
        public int Item1 { get; set; }
    }

    // Every instance of a reference type has extra two fields that are used internally by CLR.
    // Both hidden fields size is equal to the size of a pointer.
    // 8+12=20 bytes
    // 对于引用类型，返回的大小是相应类型的引用值的大小（32位系统上的4个字节），而不是存储在引用值引用的对象中的数据的大小。
    public class ReferenceType
    {
        public int Item1 { get; set; }
    }
}
