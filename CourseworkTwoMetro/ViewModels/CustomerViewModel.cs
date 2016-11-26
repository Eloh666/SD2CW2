using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public class CustomerViewModel : PropertyChangedNotifier, IDataErrorInfo
    {
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        private Customer _customer;

        public CustomerViewModel(Customer customer)
        {
            this.Customer = customer;
            this._fieldsUseDictionary = new Dictionary<string, bool>
            {
                {"Name", false},
                {"Address", false},
                {"ReferenceNumber", false}
            };

        }

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public int ReferenceNumber
        {
            get { return Customer.ReferenceNumber; }
            set
            {
                this.Customer.ReferenceNumber = value;
                OnPropertyChangedEvent("ReferenceNumber");
            }
        }

        public string Name
        {
            get { return Customer.Name; }
            set
            {
                this.Customer.Name = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent("Name");
            }
        }

        public string Address
        {
            get { return Customer.Address; }
            set
            {
                this.Customer.Address = value;
                this._fieldsUseDictionary["Address"] = true;
                OnPropertyChangedEvent("Address");
            }
        }


        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Name",
            "Address",
        };

        public bool IsCustomerValid
        {
            get
            {
                this._fieldsUseDictionary["Name"] = true;
                this._fieldsUseDictionary["Address"] = true;

                foreach (string field in ValidationFields)
                {
                    if (GetValidationError(field) != null)
                    {
                        OnPropertyChangedEvent(null);
                        return false;
                    }
                }
                OnPropertyChangedEvent(null);
                return true;
            }
        }

        // validates fields based on the requirements of the model
        private string GetValidationError(string fieldName)
        {
            string error = null;
            if (this._fieldsUseDictionary[fieldName])
            {
                switch (fieldName)
                {
                    case "Name":
                        error = this.Customer.ValidateName();
                        break;
                    case "Address":
                        error = this.Customer.ValidateAddress();
                        break;
                }
            }
            return error;
        }


    }
}