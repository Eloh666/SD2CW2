using System;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    [Serializable]
    public class Guest : Person, ICloneable
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

        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }
    }
}