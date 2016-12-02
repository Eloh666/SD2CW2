using System;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Simple customer model object
    /// </summary>
    [Serializable]
    public class Customer : Person, ICloneable
    {
       
        public int ReferenceNumber { get; set; }
        public string Address { get; set; }

        // 0 params constructor used when creating a new customer
        public Customer(){}

        // full params constructor used by the serializer
        public Customer(string name, int referenceNumber, string address) : base(name)
        {
            this.ReferenceNumber = referenceNumber;
            this.Address = address;
        }

        public string ValidateAddress()
        {
            return ValidationUtilities.ValidateNonEmpty("Address", this.Address);
        }

        // implementation of the cloning interface
        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }

    }
}