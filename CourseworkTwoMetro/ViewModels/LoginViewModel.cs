using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.API;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// View model for the login class
    /// Even if not called so (MVVM conventions) is technically a wrapper/decorator
    /// </summary>
    public class LoginViewModel : FormWithSpinnerViewModel, IDataErrorInfo
    {
        private bool _loginFailed;

        // dictionary that tracks fields used
        private readonly Dictionary<string, bool> _fieldsUseDictionary;

        // wrappeed user object
        public User User { get; }

        // inits the class adding a booking a fixing the dicionary used for validations
        public LoginViewModel()
        {
            ApiFacade.InitialiseApi();
            this._loginFailed = false;
            User = new User();
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Username", false);
            this._fieldsUseDictionary.Add("Password", false);
        }

        // wrapping getters/setters invoking the property changed notifier
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

        public bool LoginFailed
        {
            get { return this._loginFailed; }
            set
            {
                this._loginFailed = value;
                OnPropertyChangedEvent("LoginFailed");
            }
        }

        // IDataError implementation for fields validation
        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "Username",
            "Password",
        };

        // checks the validation status of the user
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