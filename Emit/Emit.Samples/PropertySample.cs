using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Emit.Samples
{
    public class PropertySample
    {
        // 使用Emit生成的最终结果类
        public class Blog
        {
            public string Title { get; set; }

            public string Content { get; set; }
        }

        public static void Run()
        {
            var blog = CreateInstance();

            blog.Title = "Emit高级特性-属性";
            blog.Content = "xxx";

            Console.WriteLine(blog.Title);
            Console.WriteLine(blog.Content);
        }

        private static dynamic CreateInstance()
        {
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Emit.Samples.Blog"), AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule("Emit.Samples.Blog");
            var typeBuilder = moduleBuilder.DefineType("Blog", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit);

            typeBuilder.DefineAutomaticProperty("Title", typeof(string));
            typeBuilder.DefineAutomaticProperty("Content", typeof(string));

            var type = typeBuilder.CreateType();

            return Activator.CreateInstance(type);
        }
    }
}
