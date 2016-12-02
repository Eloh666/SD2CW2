using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// ViewModel for the customer class.
    /// Even if not called so (MVVM conventions) is technically a wrapper/decorator
    /// </summary>
    public class CustomerViewModel : PropertyChangedNotifier, IDataErrorInfo
    {
        // dictionary that tracks fields used
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        // instance of the booking being wrapped
        private Customer _customer;

        // inits the class adding a booking a fixing the dicionary used for validations
        public CustomerViewModel(Customer customer)
        {
            this.Customer = (Customer) customer.Clone();
            this._fieldsUseDictionary = new Dictionary<string, bool>
            {
                {"Name", false},
                {"Address", false},
                {"ReferenceNumber", false}
            };

        }

        // wrapping getters/setters invoking the property changed notifier
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                _customer = (Customer) value.Clone();
                OnPropertyChangedEvent(null);
            }
        }

        public int ReferenceNumber
        {
            get { return Customer.ReferenceNumber; }
            set
            {
                this.Customer.ReferenceNumber = value;
                OnPropertyChangedEvent();
            }
        }

        public string Name
        {
            get { return Customer.Name; }
            set
            {
                this.Customer.Name = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent();
            }
        }

        public string Address
        {
            get { return Customer.Address; }
            set
            {
                this.Customer.Address = value;
                this._fieldsUseDictionary["Address"] = true;
                OnPropertyChangedEvent();
            }
        }

        // IDataError implementation for fields validation
        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Name",
            "Address",
        };

        // checks the validation status of the wrappee
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