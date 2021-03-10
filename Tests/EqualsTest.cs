using System;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class EqualsTest
    {
        //TODO: Переработать для параллельных запусков; Переопределить ==;
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
        }
        
        [Test]
        public void ValueNotEqualPersonInstanceTest()
        {
            Assert.False(_personOne.Equals(_personTwo));
        }
        
        [Test]
        public void GetHashCodeInValueEqualPersonInstanceTest()
        {
            Assert.True(_personOne.GetHashCode() == _personThree.GetHashCode());
        }

    }
}