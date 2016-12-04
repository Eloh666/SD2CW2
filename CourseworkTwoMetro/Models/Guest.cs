using System;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Guest data object model
    /// </summary>
    [Serializable]
    public class Guest : Person
    {
        public uint Age { get; set; }
        public string PassportNumber { get; set; }

        // 0 params constructor for creating a new guest
        public Guest(){}

        // full params constructor for initializing a guest from serialized json
        public Guest(string name, uint age, string passportNumber) : base(name)
        {
            this.Age = age;
            this.PassportNumber = passportNumber;
        }

        // simple validation for the guest
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