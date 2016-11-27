using System;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    [Serializable]
    public class Customer : Person, ICloneable
    {
       
        public int ReferenceNumber { get; set; }
        public string Address { get; set; }

        public Customer(){}

        public Customer(string name, int referenceNumber, string address) : base(name)
        {
            this.ReferenceNumber = referenceNumber;
            this.Address = address;
        }

        public string ValidateReferenceNumber()
        {
            return ValidationUtilities.ValidateNonEmpty("ReferenceNumber", (this.ReferenceNumber.ToString()));
        }

        public string ValidateAddress()
        {
            return ValidationUtilities.ValidateNonEmpty("Address", this.Address);
        }

        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }

    }
}