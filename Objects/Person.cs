using System;
using System.Text.RegularExpressions;

namespace TwoLayerSolution
{
    [Serializable]
    public class Person
    {
        public readonly String NameSurname;
        public readonly String DateOfBirth;
        public readonly String BirthPlace;
        public readonly String PassportNumber;
        
        public Person(String nameSurname, String dateOfBirth, String birthPlace, String passportNumber)
        {
            if (nameSurname == null || dateOfBirth == null || birthPlace == null || passportNumber == null)
                throw new ArgumentNullException("Ошибка в конструкторе Person - аргументы не могут быть null");
            
            NameSurname = nameSurname;

            DateOfBirth = dateOfBirth;
            PassportNumber = passportNumber;
            BirthPlace = birthPlace;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) throw new ArgumentNullException("Нельзя сравнивать с null.");
            
            if (obj is Person)
            {
                Person other = (Person) obj;
                return this.NameSurname == other.NameSurname && 
                       this.DateOfBirth == other.DateOfBirth && 
                       this.BirthPlace == other.BirthPlace &&
                       this.PassportNumber == other.PassportNumber;
            }
            else
            {
                throw new ArgumentException("Сравниваемый объект не Person.");
            }
        }

        public static bool operator ==(Person first, Person second)
        {
            if (first == null || second == null) throw new ArgumentNullException("Нельзя сравнивать с null.");
            return first.Equals(second);
        }

        public static bool operator !=(Person first, Person second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return (NameSurname + 
                   DateOfBirth + 
                   BirthPlace +
                   PassportNumber).GetHashCode();
        }

        public override string ToString()
        {
            return "Имя: " + NameSurname + 
                              ", Дата Рождения: " + DateOfBirth +
                              ", Место рождения " + BirthPlace +
                              ", Номер паспорта: " + PassportNumber;
        }
    }
}