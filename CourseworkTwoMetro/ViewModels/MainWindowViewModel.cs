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
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// ViewModel for the main window of the application
    /// </summary>
    public class MainWindowViewModel : FormWithSpinnerViewModel
    {
        // reference to manager singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // customer lists and selected customer
        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;
        // booking list, selected booking and booking view model instance for the left side panel invoice
        private ObservableCollection<Booking> _bookings;
        private Booking _selectedBooking;
        private BookingViewModel _bookingViewModel;
        // the tab selected on the current datagrid
        private int _selectedTabNumber;

        // construct references and updates the lists by using the API facade
        public MainWindowViewModel(MainViewModel mainViewModel)
        {
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
            // calls the refresh list command in the API facade
            this.Commands.RefreshLists(this);
        }

        // wrapping getters/setters invoking the property changed notifier
        public int SelectedTabNumber
        {
            get { return _selectedTabNumber; }
            set
            {
                _selectedTabNumber = value;
                OnPropertyChangedEvent(null);
            }
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
                OnPropertyChangedEvent();
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
                OnPropertyChangedEvent();
            }
        }

        public bool DisplaySidePanel => this.SelectedBooking != null && this.NotLoading && this.SelectedTabNumber == 0;
    }
}