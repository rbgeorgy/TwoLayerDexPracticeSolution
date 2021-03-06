using System;
using System.Text.RegularExpressions;

namespace TwoLayerSolution
{
    public class Person
    {
        public readonly String NameSurname;
        public readonly String DateOfBirth;
        public readonly String BirthPlace;
        public readonly String PassportNumber;
        
        
        private bool PassportNumberValidator(String number)
        {
            Regex rgx = new Regex("^(?!^0+$)[a-zA-Z0-9]{3,20}$");
            return rgx.IsMatch(number);
        }

        public Person(String nameSurname, String dateOfBirth, String birthPlace, String passportNumber)
        {
            if (nameSurname == null || dateOfBirth == null || birthPlace == null || passportNumber == null)
                throw new ArgumentNullException("Ошибка в конструкторе Person - аргументы не могут быть null");
            
            NameSurname = nameSurname;

            DateOfBirth = dateOfBirth;

            if (PassportNumberValidator(passportNumber)) PassportNumber = passportNumber;
            else throw new ArgumentNullException(passportNumber + " - не номер паспорта");

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

        public override int GetHashCode()
        {
            return NameSurname.GetHashCode() + 
                   DateOfBirth.GetHashCode() + 
                   BirthPlace.GetHashCode() +
                   PassportNumber.GetHashCode();
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