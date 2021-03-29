using System;
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
        public void TryAddingNonUniquePersonTest()
        {
            var catalog = new PersonCatalog();
            var generator = new PersonGenerator();
            var person = generator.GeneratePerson();
            catalog.AddPerson(person);
            
            Assert.Throws<ArgumentException>(() =>
            {
                catalog.AddPerson(person);
            });
        }

        [Test]
        public void ChangePersonFieldInCatalogTest()
        {
            var catalog = new PersonCatalog();
            var generator = new PersonGenerator();
            var person = generator.GeneratePerson();
            catalog.AddPerson(person);
            catalog.ChangePersonField(person, PersonCatalog.PersonFields.BirthPlace, "Бендеры");
            var personForCheck = new Person(person.NameSurname, person.DateOfBirth, "Бендеры", person.PassportNumber);
            
            Assert.DoesNotThrow(() =>
            {
                catalog.GetPerson(personForCheck.GetHashCode());
            });
        }

        [Test]
        public void SavePersonToHtmlTest()
        {
            var generator = new PersonGenerator();
            var person = generator.GeneratePerson();
            person.PrintToHtml();
            var filePath = Directory.GetCurrentDirectory() + "/Person" + person.GetHashCode() + ".html";
            Assert.That(filePath, Does.Exist);
        }
    }
}