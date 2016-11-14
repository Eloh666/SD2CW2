using CourseworkOneMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    public class Guest : Person
    {

        public uint Age { get; set; }
        public string PassportNumber { get; set; }

        public Guest(){}

        public Guest(string name, uint age, string passportNumber) : base(name)
        {
            this.Age = age;
            this.PassportNumber = passportNumber;
        }

        //  TODO make this one a bit smarter
        public string ValidateAge()
        {
            return ValidationUtilities.ValidateNonEmpty("Age", this.Age.ToString());
        }

        public string ValidatePassportNumber()
        {
            return ValidationUtilities.ValidateNonEmpty("PassportNumber", this.PassportNumber);
        }

    }
}