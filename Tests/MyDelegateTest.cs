using System;
using System.Collections.Generic;
using System.Reflection;
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
            throw new Exception("Исключение");
        }

        public int ChangeSecondInt(int x, ref int a)
        {
            a = x;
            return a;
        }

        [Test]
        public void IgnoreExceptionTest()
        {
            var myDelegate = new MyDelegate();
            int secondInt = 12;
            
            var type = Type.GetType("Tests.MyDelegateTest");
            var classInstance = Activator.CreateInstance(type);
            MethodInfo multiplicationMethodInfo = type.GetMethod("Multiplication");
            myDelegate.AddMethod(multiplicationMethodInfo);
            Assert.AreEqual(120, myDelegate.Invoke(classInstance, new object[]{10, secondInt}));
            
            MethodInfo methodThatThrowsInfo = type.GetMethod("MethodThatThrows");
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.AddMethod(methodThatThrowsInfo);
            myDelegate.Invoke(classInstance, new object[] {10, secondInt});
            
            MethodInfo changeSecondIntInfo = type.GetMethod("ChangeSecondInt");
            myDelegate.AddMethod(changeSecondIntInfo);

            Console.WriteLine(myDelegate.Invoke(classInstance, new object[] {10, secondInt}));
        }
    }
}