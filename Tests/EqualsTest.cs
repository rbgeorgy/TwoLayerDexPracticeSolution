using System;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class EqualsTest
    {
        private Person _personOne;
        private Person _personTwo;
        private Person _personThree;

        [SetUp]
        public void SetUp()
        {
            _personOne = new Person(
                "Ivanov Ivan Ivanovich",
                "01.01.1990",
                "Nezavertailovka",
                "AA2281337"
            );
            
            _personTwo = new Person(
                "Alekseyev Artyom Ivanovich",
                "21.01.1991",
                "Moscow",
                "AA1488146"
            );
            
            _personThree = new Person(
                "Ivanov Ivan Ivanovich",
                "01.01.1990",
                "Nezavertailovka",
                "AA2281337"
            );
        }

        [Test]
        public void ValueEqualPersonInstanceTest()
        {
            Assert.True(_personOne.Equals(_personThree));
            Console.WriteLine("personOne Equal to personThree? " + _personOne.Equals(_personThree));
        }
        
        [Test]
        public void GetHashCodeInValueEqualPersonInstanceTest()
        {
            Assert.True(_personOne.GetHashCode() == _personThree.GetHashCode());
            Console.WriteLine("GetHashCode _personOne возвращает " + _personOne.GetHashCode());
            Console.WriteLine("GetHashCode _personTwo возвращает " + _personTwo.GetHashCode());
            Console.WriteLine("GetHashCode _personThree возвращает " + _personThree.GetHashCode());
        }

    }
}