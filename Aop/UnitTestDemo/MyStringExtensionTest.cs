using NUnit.Framework;

namespace UnitTestDemo
{
    public class MyStringExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var myStrObj = new MyStringExtension();
            var reversedStr = myStrObj.Reverse("hello");
            Assert.That(reversedStr, Is.EqualTo("olleh")); 
        }
    }
}