using System.IO;
using NUnit.Framework;
using TwoLayerSolution;

namespace Tests
{
    public class PersonCatalogTest
    {
        [Test]
        public void SaveLoadPersonCatalogTest()
        {
            var catalog = new PersonCatalog();
            var generator = new PersonGenerator();

            for (int i = 0; i < 10; i++)
            {
                var person = generator.GeneratePerson();
                catalog.AddPerson(person);   
            }
            catalog.Save();
            catalog.Load(Directory.GetCurrentDirectory() + "/myDictionary.txt");
            Assert.AreEqual(10, catalog.Count());
        }
        
        [Test]
        public void 
    }
}