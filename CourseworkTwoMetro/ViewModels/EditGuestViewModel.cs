using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// ViewModel for the guest class.
    /// Even if not called so (MVVM conventions) is technically a wrapper/decorator
    /// </summary>
    public class EditGuestViewModel : PropertyChangedNotifier, IDataErrorInfo
    {
        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // list of guests of the current booking
        public ObservableCollection<Guest> Guests { get; set; }

        // dictionary that tracks fields used
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        // title of the of the window
        public string Title { get; set; }
        // the current guest being edited and cloned from the original one
        public Guest Guest { get; set; }
        // the original guest being set if editing
        public Guest SelectedGuest { get; set; }
        public EditGuestViewModel(string title, MainViewModel mainViewModel, ObservableCollection<Guest> guests, Guest guest = null)
        {
            // instance of the manager singletons
            this.Windows = WindowsManager.Instance(mainViewModel);
            this.Commands = CommandsManager.Instance(mainViewModel);
            // title of the of the window
            this.Title = title;
            // clones the selected guest and sets the original if editing
            if (guest == null)
            {
                this.Guest = new Guest();
                this.SelectedGuest = null;
            }
            else
            {
                this.Guest = (Guest) guest.Clone();
                this.SelectedGuest = guest;
            }
            // sets up the validation for the dictionary
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Name", false);
            this._fieldsUseDictionary.Add("Age", false);
            this._fieldsUseDictionary.Add("PassportNumber", false);
            this.Guests = guests;
        }

        // wrapping getters/setters invoking the property changed notifier
        public void AddGuest(Guest guest)
        {
            this.Guests.Add(guest);
            OnPropertyChangedEvent(null);
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

        // IDataError implementation for fields validation
        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Name",
            "Age",
            "PassportNumber"
        };

        // checks the validation status of the wrappee
        public bool IsGuestValid
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