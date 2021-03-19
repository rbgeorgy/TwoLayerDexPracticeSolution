using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class ReflectionTest
    {
        private object GetInstanceOfTriangle(string strFullyQualifiedName,double aSide, double bSide, double cSide)
        {         
            var type = Type.GetType(strFullyQualifiedName); 
            return  Activator.CreateInstance(type ?? throw new InvalidOperationException(), aSide, bSide, cSide);         
        }

        [Test]
        public void CreateTriangleFromStringTest()
        {
            string triangleName = "TwoLayerSolution.Triangle";
            var item = GetInstanceOfTriangle(triangleName, 3, 4, 5);
            Assert.AreEqual("Треугольник", item.ToString());
        }

        [Test]
        public void MethodWithParametersTest()
        {
            string className = "TwoLayerSolution.PersonGenerator";
            var type = Type.GetType(className);
            
            string methodName = "GeneratePersonArray";
            
            if (!(type is null))
            {
                var classInstance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(methodName);
                var result = methodInfo?.Invoke(classInstance, new object[] {3});
                int counter = 0;
                if (result != null)
                {
                    foreach (var item in (IEnumerable) result)
                    {
                        counter++;
                        Console.WriteLine(item);
                    }
                    Assert.AreEqual(3, counter);
                }
                else
                {
                    throw new NullReferenceException("methodInfo.Invoke == null");
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        [Test]
        public void TryingGetPrivatePropertyHavingItsNameTest()
        {
            var classToDemonstrateReflection = new ClassToDemonstrateReflection();
            var privateProperty = typeof(ClassToDemonstrateReflection).GetProperty("StringProperty",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (!(privateProperty is null))
            {
                var getter = privateProperty.GetGetMethod(nonPublic: true);
                var result = getter?.Invoke(classToDemonstrateReflection, null);
                Console.WriteLine(result);
                Assert.AreEqual("Это значение приватного свойства.", result);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        
        [Test]
        public void TryingGetPrivatePropertyDontHavingItsNameTest()
        {
            var classToDemonstrateReflection = new ClassToDemonstrateReflection();
            var getPropertiesResult = typeof(ClassToDemonstrateReflection).GetProperties(BindingFlags.NonPublic|BindingFlags.Instance);
            string privatePropertyName = "";
            foreach (var propertyInfo in getPropertiesResult)
            {
                privatePropertyName = propertyInfo.Name;
            }

            Assert.AreEqual("StringProperty",privatePropertyName);
            var privateProperty = typeof(ClassToDemonstrateReflection).GetProperty(privatePropertyName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (!(privateProperty is null))
            {
                var getter = privateProperty.GetGetMethod(nonPublic: true);
                var result = getter?.Invoke(classToDemonstrateReflection, null);
                Assert.AreEqual("Это значение приватного свойства.", result);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}