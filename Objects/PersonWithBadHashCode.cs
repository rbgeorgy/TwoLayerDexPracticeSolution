namespace TwoLayerSolution
{
    public class PersonWithBadHashCode : Person
    {
        public PersonWithBadHashCode(string nameSurname, string dateOfBirth, string birthPlace, string passportNumber) 
            : base(nameSurname, dateOfBirth, birthPlace, passportNumber)
        {
        }
        public override int GetHashCode()
        {
            return 13371488;
        }
    }
}