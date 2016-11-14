using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.API;
using Xceed.Wpf.DataGrid.Converters;

namespace CourseworkTwoMetro.ViewModels
{
    public class MainWindowViewModel : FormWithSpinnerViewModel
    {
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Booking> _bookings;

        public MainWindowViewModel()
        {
            this.RefreshLists();
        }

        public async void RefreshLists()
        {
            this.Loading = true;
            try
            {
                this.Customers = await ApiFacade.GetCustomers();
                this.Bookings = await ApiFacade.GetBookings();
                this.LoadingFailed = false;
            }
            catch
            {
                this.LoadingFailed = true;
            }
            finally
            {
                this.Loading = false;
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
    }
}