using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// ViewModel for the wizard edit view model class, it is used for the edit booking wizard.
    /// This is basically facade that encapsulates a few different view models. Due to MVVM 
    /// convention its better howerver to call it "viewModel"
    /// </summary>
    public class EditBookingViewModel : FormWithSpinnerViewModel, IDataErrorInfo
    {
        public string Title { get; }

        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // customers and bookings lists
        public ObservableCollection<Customer> Customers { get; }
        public ObservableCollection<Booking> Bookings { get; }

        // status of the radio buttons
        private bool _newCustomerRadio;
        private bool _existingCustomerRadio;

        // validation dictionary
        private readonly Dictionary<string, bool> _fieldsUseDictionary;

        // wizard page two bindings
    
        // customer being edited
        public BookingViewModel NewBooking { get; set; }
        // original customer, if any
        public Booking OriginalBooking { get; set; }
        // guest being edited
        public Guest NewGuest { get; set; }
        // guest selected on the table
        public Guest SelectedGuest { get; set; }
        // switches for the extras
        private bool _carHireSwitch;
        private bool _breakfastSwitch;
        private bool _dinnerSwitch;
        // list of guests
        private ObservableCollection<Guest> _currentGestsList;
        private Customer _existingCustomer;
        private CustomerViewModel _newCustomer;
        // constants for the window
        public DateTime MinStartDate { get; }
        public DateTime MinEndDate { get; }

        // new booking
        public EditBookingViewModel(string title, MainViewModel mainViewModel, BookingViewModel selectedBooking = null)
        {
            // initial setup of the validation dictionary
            this._fieldsUseDictionary = new Dictionary<string, bool>
            {
                {"Name", false},
                {"Address", false}
            };
            // status of the booking (adding or editing)
            this.NewBooking = selectedBooking ?? new BookingViewModel(new Booking());
            this.OriginalBooking = selectedBooking?.Booking;
            // status of the extras flags
            if (selectedBooking != null)
            {
                this.BreakfastSwitch = this.NewBooking.Breakfast != null;
                this.DinnerSwitch = this.NewBooking.Dinner != null;
                this.CarHireSwitch = this.NewBooking.CarHire != null;
            }
            // title
            this.Title = title;
            // manager singletons
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
            // lists
            this.Customers = mainViewModel.MainWindowViewModel.Customers;
            this.Bookings = mainViewModel.MainWindowViewModel.Bookings;
            // radio button(s)
            this.CreateNewCustomer = true;

            // constants
            this.MinStartDate = DateTime.Today;
            this.MinEndDate = DateTime.Today.AddDays(1);

            // customer to be added
            this.NewCustomer = new CustomerViewModel(new Customer());
            if (this.NewBooking.CustomerId == 0) return;
            // if editing an existing booking, select the customer based on the customer ID
            // of the booking being edited
            foreach (var customer in Customers)
            {
                if (customer.ReferenceNumber != NewBooking.CustomerId) continue;
                this.ExistingCustomer = customer;
                this._existingCustomerRadio = true;
                break;
            }
            OnPropertyChangedEvent(null);
        }

        // wrapping getters/setters invoking the property changed notifier
        public string Name
        {
            get { return this._newCustomer.Name; }
            set
            {
                this._newCustomer.Name = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent(null);
            }
        }

        public string Address
        {
            get { return this._newCustomer.Address; }
            set
            {
                this._newCustomer.Address = value;
                this._fieldsUseDictionary["Name"] = true;
                OnPropertyChangedEvent(null);
            }
        }

        public CustomerViewModel NewCustomer
        {
            get { return _newCustomer; }
            set
            {
                _newCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public Customer ExistingCustomer
        {
            get { return _existingCustomer; }
            set
            {
                _existingCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool DieteryReqsShow => this.BreakfastSwitch || this.DinnerSwitch;

        public bool CreateNewCustomer
        {
            get { return _newCustomerRadio; }
            set
            {
                _newCustomerRadio = value;
                _existingCustomerRadio = !value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool UseExistingCustomer
        {
            get { return _existingCustomerRadio; }
            set
            {
                _existingCustomerRadio = value;
                _newCustomerRadio = !value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool CarHireSwitch
        {
            get { return _carHireSwitch; }
            set
            {
                _carHireSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool BreakfastSwitch
        {
            get { return _breakfastSwitch; }
            set
            {
                _breakfastSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool DinnerSwitch
        {
            get { return _dinnerSwitch; }
            set
            {
                _dinnerSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public ObservableCollection<Guest> CurrentGestsList
        {
            get { return _currentGestsList; }
            set
            {
                _currentGestsList = value;
                OnPropertyChangedEvent(null);
            }
        }

        // wizard bindings used for navigating pages or denying navigation to pages
        public bool CanMoveToPageTwo => this._existingCustomerRadio ? this.ExistingCustomer != null : this.NewCustomer.IsCustomerValid;
        public bool CanMoveToSavePage => this.NewBooking.Guests.Count > 0;

        public bool CanMoveBackToPageTwo => !this.CanFinish;
        public bool CanFinish => this.LoadingSuccess || this.LoadingFailed;

        // IDataError implementation for fields validation
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
            if (fieldName != "Name" && fieldName != "Address")
            {
                return null;
            }
            string error = null;
            if (this._fieldsUseDictionary != null && this._fieldsUseDictionary[fieldName])
            {
                switch (fieldName)
                {
                    case "Name":
                        error = this.NewCustomer.Customer.ValidateName();
                        break;
                    case "Address":
                        error = this.NewCustomer.Customer.ValidateAddress();
                        break;
                }
            }
            return error;
        }
    }
}