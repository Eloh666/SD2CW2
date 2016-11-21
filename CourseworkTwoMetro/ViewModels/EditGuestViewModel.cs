using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class EditGuestViewModel : FormWithSpinnerViewModel, IDataErrorInfo
    {
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        public string Title { get; set; }
        public Guest Guest { get; }
        public EditGuestViewModel(string title, Guest guest = null)
        {
            this.Title = title;
            this.Guest = guest ?? new Guest();
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Name", false);
            this._fieldsUseDictionary.Add("Age", false);
            this._fieldsUseDictionary.Add("PassportNumber", false);
        }

        public string Name
        {
            get { return Guest.Name; }
            set
            {
                Guest.Name = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent("Name");
            }
        }

        public uint Age
        {
            get { return Guest.Age; }
            set
            {
                Guest.Age = value;
                this._fieldsUseDictionary["Age"] = true;
                OnPropertyChangedEvent("Age");
            }
        }

        public string PassportNumber
        {
            get { return Guest.PassportNumber; }
            set
            {
                Guest.PassportNumber = value;
                this._fieldsUseDictionary["PassportNumber"] = true;
                OnPropertyChangedEvent("PassportNumber");
            }
        }

        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Name",
            "Age",
            "PassportNumber"
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
                this._fieldsUseDictionary["PassportNumber"] = true;


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
                        error = this.Guest.ValidateName();
                        break;
                    case "Age":
                        error = this.Guest.ValidateAge();
                        break;
                    case "PassportNumber":
                        error = this.Guest.ValidatePassportNumber();
                        break;
                }
            }
            return error;
        }
    }
}