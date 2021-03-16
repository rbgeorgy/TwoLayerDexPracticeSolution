using System;
using TwoLayerSolution;

namespace SimpleCatalog
{
    class Program
    {
        private static void ReadInfo(EmployeeCatalog catalog)
        {
            Console.WriteLine("Введите ФИО искомого работника: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите дату рождения искомого работника: ");
            string date = Console.ReadLine();
            Console.WriteLine("Введите место рождения искомого работника: ");
            string place = Console.ReadLine();
            Console.WriteLine("Введите номер паспорта искомого работника: ");
            string number = Console.ReadLine();

            try
            {
                catalog.MakeRequest(name, date, place, number);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            var personGenerator = new PersonGenerator();
            var catalog = new EmployeeCatalog(personGenerator.GeneratePersonWorkPlaceDictionary(1000));
            while (true)
            {
                ReadInfo(catalog);
            }
        }
    }
}