using System;
using System.Collections.Generic;
using System.IO;

namespace TwoLayerSolution
{
    public class PersonCatalog
    {
        private Dictionary<int, Person> _personDictionary;

        public PersonCatalog()
        {
            _personDictionary = new Dictionary<int, Person>();
        }

        public enum PersonFields
        {
            NameSurname,
            DateOfBirth,
            BirthPlace,
            PassportNumber
        }

        private void CheckForExistence(Person person)
        {
            if (_personDictionary.ContainsKey(person.GetHashCode()))
            {
                throw new ArgumentException("Person не был уникальным. ");
            }
        }

        public void AddPerson(Person person)
        {
            var id = person.GetHashCode();
            try
            {
                _personDictionary.Add(id, person);
            }
            catch (Exception e)
            {
                Console.WriteLine("Person не был уникальным. " + e);
                throw;
            }
        }

        public void DeletePerson(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (_personDictionary.ContainsKey(person.GetHashCode()))
            {
                _personDictionary.Remove(person.GetHashCode());
            }
        }

        public void ChangePersonToPerson(Person oldPerson, Person newPerson)
        {
            DeletePerson(oldPerson);
            AddPerson(newPerson);
        }

        public void ChangePersonName(Person person, PersonFields fieldToChange, string value)
        {
            var id = person.GetHashCode();
            if (!_personDictionary.ContainsKey(id)) return;
            Person newPerson;
            switch (fieldToChange) 
            {
                case PersonFields.NameSurname:
                    newPerson = new Person(value, person.DateOfBirth, person.BirthPlace, person.PassportNumber);
                    AddPerson(newPerson);
                    break;
                case PersonFields.DateOfBirth:
                    newPerson = new Person(person.NameSurname, value, person.BirthPlace, person.PassportNumber);
                    AddPerson(newPerson);
                    break;
                case PersonFields.BirthPlace:
                    newPerson = new Person(person.NameSurname, person.DateOfBirth, value, person.PassportNumber);
                    AddPerson(newPerson);
                    break;
                case PersonFields.PassportNumber:
                    newPerson = new Person(person.NameSurname, person.DateOfBirth, person.BirthPlace, value);
                    AddPerson(newPerson);
                    break;
            }
            _personDictionary.Remove(id);
        }

        public void Save()
        {
            var directory = Directory.GetCurrentDirectory();
            using (var file = new StreamWriter(directory + @"/myDictionary.txt"))
                foreach (var person in _personDictionary)
                    file.WriteLine("[{0} {1}]", person.Key, person.Value);
            Console.WriteLine(directory);
        }

        public void Load(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var newBase = new Dictionary<int, Person>();
                try
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        var tuple = ParseLine(line);
                        newBase.Add(tuple.Item1, tuple.Item2);
                        line = reader.ReadLine();
                    }
                    _personDictionary = newBase;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Проблемы с записью. " + e);
                }
            }
        }

        public int Count()
        {
            return _personDictionary.Count;
        }

        private Tuple<int, Person> ParseLine(string line)
        {
            var words = line.Split(' ');
            var person = new Person(
                words[2] + " " + words[3] + " " + words[4],
                words[7],
                words[10],
                words[13]
                );
            return new Tuple<int, Person>(Int32.Parse(words[0].Remove(0, 1)), person);
        }

    }
}