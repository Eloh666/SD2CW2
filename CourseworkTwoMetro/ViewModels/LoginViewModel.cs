using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseworkOneMetro.ViewModels.Utils;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public class LoginViewModel : PropertyChangedNotifier, IDataErrorInfo
    {
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        public User User { get; }
        public CommandsManager Commands { get; }

        public LoginViewModel()
        {
            ApiFacade.InitialiseApi();
            User = new User();
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Username", false);
            this._fieldsUseDictionary.Add("Password", false);
            this.Commands = new CommandsManager();
        }

        public string Username
        {
            get { return User.Username; }
            set
            {
                User.Username = value;
                this._fieldsUseDictionary["Username"] = true;
                OnPropertyChangedEvent("Username");
            }
        }

        public string Password
        {
            get { return User.Password; }
            set
            {
                User.Password = value;
                this._fieldsUseDictionary["Password"] = true;
                OnPropertyChangedEvent("Password");
            }
        }

        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Username",
            "Password",
        };

        // check the whole attendee for being valid, the dictionary is used
        // to avoid displaying the validation errors until a field has actually been used
        // at least once or the user has tried to save/open an extra window
        public bool IsUserValid
        {
            get
            {
                this._fieldsUseDictionary["Username"] = true;
                this._fieldsUseDictionary["Password"] = true;


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
                    case "Username":
                        error = this.User.ValidateUsername();
                        break;
                    case "Password":
                        error = this.User.ValidatePassword();
                        break;
                }
            }
            return error;
        }
    }  
}