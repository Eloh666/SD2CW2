using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class EditCustomerViewModel : FormWithSpinnerViewModel, IDataErrorInfo
    {
        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        public string Title { get; set; }
        public Customer Customer { get; }
        public EditCustomerViewModel(string title, Customer customer = null)
        {
            this.Title = title;
            this.Customer = customer ?? new Customer();
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Name", false);
            this._fieldsUseDictionary.Add("ReferenceNumber", false);
            this._fieldsUseDictionary.Add("Address", false);
        }

        public string Name
        {
            get { return Customer.Name; }
            set
            {
                Customer.Name = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent("Name");
            }
        }

        public int ReferenceNumber
        {
            get { return Customer.ReferenceNumber; }
            set
            {
                Customer.ReferenceNumber = value;
                this._fieldsUseDictionary["ReferenceNumber"] = true;
                OnPropertyChangedEvent("ReferenceNumber");
            }
        }

        public string Address
        {
            get { return Customer.Address; }
            set
            {
                Customer.Address = value;
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
            "ReferenceNumber",
            "Address"
        };

        // check the whole attendee for being valid, the dictionary is used
        // to avoid displaying the validation errors until a field has actually been used
        // at least once or the user has tried to save/open an extra window
        public bool IsUserValid
        {
            get
            {
                this._fieldsUseDictionary["Name"] = true;
                this._fieldsUseDictionary["Password"] = true;
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
                    case "ReferenceNumber":
                        error = this.Customer.ValidateReferenceNumber();
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