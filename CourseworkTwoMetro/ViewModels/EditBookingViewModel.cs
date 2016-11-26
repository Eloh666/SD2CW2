using System.Collections.ObjectModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class EditBookingViewModel : FormWithSpinnerViewModel
    {
        public string Title { get; }

        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // customers and bookings lists
        public ObservableCollection<Customer> Customers { get; }
        public ObservableCollection<Booking> Bookings { get; }

        // wizard page one bindings
        public Customer NewCustomer { get; set; }
        public Customer ExistingCustomer { get; set; }
        private bool _newCustomerRadio;
        private bool _existingCustomerRadio;

        // wizard page two bindings
        public Booking NewBooking { get; set; }
        public Guest NewGuest { get; set; }
        public Guest SelectedGuest { get; set; }
        private bool _carHireSwitch;
        private bool _breakfastSwitch;
        private bool _dinnerSwitch;
        private ObservableCollection<Guest> _currentGestsList;

        // new booking
        public EditBookingViewModel(string title, MainViewModel mainViewModel)
        {
            this.Title = title;
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
            this.Customers = mainViewModel.MainWindowViewModel.Customers;
            this.Bookings = mainViewModel.MainWindowViewModel.Bookings;
            this.CreateNewCustomer = true;

            if (this.NewBooking == null)
            {
                this.NewBooking = new Booking();
            }          
            this.NewCustomer = new Customer();
            if (this.NewBooking.CustomerId == 0) return;
            foreach (var customer in Customers)
            {
                if (customer.ReferenceNumber != NewBooking.CustomerId) continue;
                this.ExistingCustomer = customer;
                break;
            }
        }

        //edit booking
        public EditBookingViewModel(string title, MainViewModel mainViewModel, Booking selectedBooking) : this(title, mainViewModel)
        {
            this.NewBooking = selectedBooking;
            this.ExistingCustomer = null;
            
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
                OnPropertyChangedEvent("CarHireSwitch");
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
                OnPropertyChangedEvent("CurrentGestsList");
            }
        }
    }
}