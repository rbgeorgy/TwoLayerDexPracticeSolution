using System;
using System.Collections.Generic;

namespace TwoLayerSolution
{
    public class EmployeeCatalog
    {
        private Dictionary<Person, string> _placeOfWorkDirectory;

        public EmployeeCatalog(Dictionary<Person, string> placeOfWorkDirectory)
        {
            _placeOfWorkDirectory = placeOfWorkDirectory ?? throw new ArgumentNullException("Словарь не может быть null.");
        }

        public EmployeeCatalog()
        {
            _placeOfWorkDirectory = new Dictionary<Person, string>();
        }

        public bool MakeRequest(string name, string date, string place, string number)
        {
            if (name == null || date == null || place == null || number == null)
            {
                throw new ArgumentNullException("Заполните аргументы запроса!");
            }
            else
            {
                Person query = new Person(name, date, place, number);
                if (_placeOfWorkDirectory.ContainsKey(query))
                {
                    Console.WriteLine("Работник " + query.NameSurname + " найден! Место работы: " + _placeOfWorkDirectory[query]);
                    return true;
                }
                else
                {
                    Console.WriteLine("Работника " + query.NameSurname + " нет в базе.");
                }
            }
            return false;
        }

        public void AddItem(Person key, string value)
        {
            if (key == null || value == null)
            {
                throw new ArgumentNullException("Заполните аргументы запроса!");
            }
            else
            {
                _placeOfWorkDirectory[key] = value;
            }
        }
    }
}