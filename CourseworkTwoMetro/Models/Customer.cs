using CourseworkOneMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    public class Customer : Person
    {
       
        public string ReferenceNumber { get; set; }
        public string Address { get; set; }

        public Customer(){}

        public Customer(string name, string referenceNumber, string address) : base(name)
        {
            this.ReferenceNumber = referenceNumber;
            this.Address = address;
        }

        public string ValidateReferenceNumber()
        {
            return ValidationUtilities.ValidateNonEmpty("ReferenceNumber", this.ReferenceNumber);
        }

        public string ValidateAddress()
        {
            return ValidationUtilities.ValidateNonEmpty("Address", this.Address);
        }

    }
}