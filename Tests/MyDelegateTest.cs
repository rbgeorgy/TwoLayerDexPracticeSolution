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

        public void ReferenceIncrement(ref int a)
        {
            a++;
        }
        
        public void MethodWithReferenceIncrementSignatureButItThrows(ref int a)
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
            myDelegate += new MyDelegate(methodThatThrowsInfo);
            myDelegate += new MyDelegate(methodThatThrowsInfo);
            myDelegate += new MyDelegate(methodThatThrowsInfo);
            
            var result = myDelegate.Invoke(this, new object[] {32, 10});
            Assert.AreEqual(320, (int) result);
            Console.WriteLine("Последний результат, игнорируя исключения: " + result);
        }

        [Test]
        public void IgnoreExceptionAndCallReferenceFunctionTest()
        {
            var type = Type.GetType(this.ToString());
            Debug.Assert(type != null, nameof(type) + " != null");
            
            var referenceIncrementInfo = type.GetMethod(nameof(this.ReferenceIncrement));
            var methodThatThrowsInfo = type.GetMethod(nameof(this.MethodWithReferenceIncrementSignatureButItThrows));
            
            var myDelegate = new MyDelegate(referenceIncrementInfo);
            
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.AddMethod(methodThatThrowsInfo);
            
            myDelegate.AddMethod(referenceIncrementInfo);
            
            var arrayWithResult = new object[] {0};

            myDelegate.Invoke(this, arrayWithResult);
            Assert.AreEqual(2, arrayWithResult[0]);
        }
    }
}