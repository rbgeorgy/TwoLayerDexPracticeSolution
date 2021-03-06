using System.Collections.Generic;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class DictionaryTest
    {
        private Person _personOne;
        private Person _personTwo;
        private Person _personThree;
        private EmployeeCatalog _employeeCatalog;

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
                "Nesterov Dmitry Sergeevich",
                "12.01.1990",
                "Sankt-Petersburg",
                "AA4515515"
            );
            
            _employeeCatalog = new EmployeeCatalog(new Dictionary<Person, string>
            {
                {_personOne, "Tiraet"},
                {_personTwo, "JetBrains"},
                {_personThree, "SPBSU"}
            });
        }
        
        [Test]
        public void RequestForExistingItemInCatalogTest()
        {
            Assert.True(_employeeCatalog.MakeRequest("Nesterov Dmitry Sergeevich", "12.01.1990", "Sankt-Petersburg", "AA4515515"));
        }
        
        [Test]
        public void RequestForNotExistingItemInCatalogTest()
        {
            Assert.True(!_employeeCatalog.MakeRequest("Nesterov Oleg Sergeevich", "12.01.1990", "Sankt-Petersburg", "AA4515515"));
        }
    }
}