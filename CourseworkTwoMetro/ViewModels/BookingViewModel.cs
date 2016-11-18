using System.Collections.ObjectModel;
using System.ComponentModel;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public class BookingViewModel : PropertyChangedNotifier
    {
        // customers and bookings lists
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Booking> _bookings;

        // wizard page one bindings
        public Customer NewCustomer { get; set; }
        private bool _newCustomerRadio;
        private bool _existingCustomerRadio;

        // wizard page two bindings
        public Booking NewBooking { get; set; }
        public Guest TempNewBooking { get; set; }
        private bool _carHireSwitch;
        private bool _breakfastSwitch;
        private bool _dinnerSwitch;
        private ObservableCollection<Guest> _currentGestsList;

        public ObservableCollection<Customer> Customers => _customers;
        public ObservableCollection<Booking> Bookings => _bookings;

        

        public bool NewCustomerRadio
        {
            get { return _newCustomerRadio; }
            set
            {
                _newCustomerRadio = value;
                _existingCustomerRadio = !value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool ExistingCustomerRadio
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
                OnPropertyChangedEvent("BreakfastSwitch");
            }
        }

        public bool DinnerSwitch
        {
            get { return _dinnerSwitch; }
            set
            {
                _dinnerSwitch = value;
                OnPropertyChangedEvent("DinnerSwitch");
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