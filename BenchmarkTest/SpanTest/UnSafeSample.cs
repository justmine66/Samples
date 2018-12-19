using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpanTest
{
    public class UnSafeSample
    {
        public static void Test()
        {
            //Read();
            //Write();
            //AsPointer();
            //CopyBlock();
            //InitBlock();
            //CouldUseNewUnsafePackage();
            StringOp();
        }

        public static unsafe void Read()
        {
            var ptr = stackalloc int[1];
            *ptr = 42;

            var v = Unsafe.Read<int>(ptr);
        }

        public static unsafe void Write()
        {
            var ptr = stackalloc int[1];
            *ptr = 17;

            Unsafe.Write(ptr, 20);

            var v = ptr[0];
        }

        public static unsafe void AsPointer()
        {
            var array = new int[2];
            array[0] = 42;
            array[1] = 17;
            fixed (void* pinPtr = array)
            {
                var firstPtr = Unsafe.AsPointer(ref array[0]);

                var one = Unsafe.Read<int>(firstPtr);
                var two = Unsafe.Read<int>((byte*)firstPtr + sizeof(int));
            }
        }

        public static unsafe void CopyBlock()
        {
            var src = stackalloc byte[2];
            src[0] = src[1] = 0x12;
            var dest = stackalloc short[1];
            Unsafe.CopyBlock(dest, src, 2);
            var v = dest[0];
        }

        public static unsafe void InitBlock()
        {
            var ptr = stackalloc byte[10];
            Unsafe.InitBlock(ptr, 123, 10);
            for (var i = 0; i < 10; i++)
                Console.WriteLine(ptr[i]);
        }

        public static unsafe void CouldUseNewUnsafePackage()
        {
            var dt = new KeyValuePair<DateTime, decimal>[2];
            dt[0] = new KeyValuePair<DateTime, decimal>(DateTime.UtcNow.Date, 123.456M);
            dt[1] = new KeyValuePair<DateTime, decimal>(DateTime.UtcNow.Date.AddDays(1), 789.101M);

            var obj = (object)dt;
            var asBytes = Unsafe.As<byte[]>(obj);
            fixed (byte* ptr = &asBytes[0])
            {
                for (var i = 0; i < (8 + 16) * 2; i++)
                {
                    Console.WriteLine(*(ptr + i));
                }

                var firstDate = *(DateTime*)ptr;
                Console.WriteLine(firstDate);
                var firstDecimal = *(decimal*)(ptr + 8);
                Console.WriteLine(firstDecimal);
                var secondDate = *(DateTime*)(ptr + 8 + 16);
                Console.WriteLine(secondDate);
                var secondDecimal = *(decimal*)(ptr + 8 + 16 + 8);
                Console.WriteLine(secondDecimal);
            }
        }

        public static unsafe void StringOp()
        {
            var text = "ABCDEFGHIJKLMNOPQRSTUVWXKZ";

            Console.WriteLine("String Length {0}", text.Length); // prints 26
            Console.WriteLine("Text: \"{0}\"", text); // "ABCDEFGHIJKLMNOPQRSTUVWXKZ"

            var pinnedText = GCHandle.Alloc(text, GCHandleType.Pinned);
            var textAddress = (char*)pinnedText.AddrOfPinnedObject().ToPointer();

            // Make an immutable string think that it is shorter than it actually is!!!
            Unsafe.Write(textAddress - 2, 5);

            Console.WriteLine("String Length {0}", text.Length); // prints 5
            Console.WriteLine("Text: \"{0}\"", text); // prints "ABCDE

            // change the 2nd character 'B' to '@'
            Unsafe.Write(textAddress + 1, '@');

            Console.WriteLine("Text: \"{0}\"", text); // prints "A@CDE

            pinnedText.Free();
        }
    }
}
