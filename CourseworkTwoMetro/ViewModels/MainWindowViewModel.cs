using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.API;
using Xceed.Wpf.DataGrid.Converters;

namespace CourseworkTwoMetro.ViewModels
{
    public class MainWindowViewModel : FormWithSpinnerViewModel
    {
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;
        private ObservableCollection<Booking> _bookings;
        private Booking _selectedBooking;
        private BookingViewModel _bookingViewModel;
        public int SelectedTabNumber { get; set; }

        public MainWindowViewModel(MainViewModel mainViewModel)
        {
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
            this.Commands.RefreshLists(this);
        }

        public BookingViewModel BookingViewModel
        {
            get { return _bookingViewModel; }
            set
            {
                _bookingViewModel = value;
                OnPropertyChangedEvent(null);

            }
        }

        public Customer SelectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                this._selectedCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public Booking SelectedBooking
        {
            get
            {
                return this._selectedBooking;
            }
            set
            {
                this._selectedBooking = value;
                OnPropertyChangedEvent(null);
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChangedEvent("Customers");
            }
        }

        public ObservableCollection<Booking> Bookings
        {
            get
            {
                return _bookings;
            }
            set
            {
                _bookings = value;
                OnPropertyChangedEvent("Bookings");
            }
        }

        public bool DisplayStockPanel => this.SelectedBooking != null && this.NotLoading;
    }
}