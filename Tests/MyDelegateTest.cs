using System;
using System.Diagnostics;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class MyDelegateTest
    {

        public int Multiplication(int a, int b)
        {
            return a * b;
        }

        public int MethodThatThrows(int a, int b)
        {
            throw new Exception("Внезапное исключение в стеке вызовов!");
        }

        [Test]
        public void SimpleMyDelegateTest()
        {
            var type = Type.GetType(this.ToString());
            Debug.Assert(type != null, nameof(type) + " != null");
            var multiplicationMethodInfo = type.GetMethod(nameof(this.Multiplication));
            
            var myDelegate = new MyDelegate(multiplicationMethodInfo);
            Assert.AreEqual(120, myDelegate.Invoke(this, new object[]{10, 12}));
        }

        [Test]
        public void IgnoreExceptionTest()
        {
            var type = Type.GetType(this.ToString());
            Debug.Assert(type != null, nameof(type) + " != null");
            
            var multiplicationMethodInfo = type.GetMethod(nameof(this.Multiplication));
            var myDelegate = new MyDelegate(multiplicationMethodInfo);

            var methodThatThrowsInfo = type.GetMethod(nameof(this.MethodThatThrows));
            myDelegate -= myDelegate;
            myDelegate += new MyDelegate(methodThatThrowsInfo);
            myDelegate += new MyDelegate(methodThatThrowsInfo);
            myDelegate += myDelegate;

            var anotherMyDelegate = new MyDelegate(multiplicationMethodInfo);
            myDelegate += anotherMyDelegate;

            var result = myDelegate.Invoke(this, new object[] {32, 10});
            Assert.AreEqual(320, (int) result);
            Console.WriteLine("Результат после исключений: " + result);
        }
    }
}