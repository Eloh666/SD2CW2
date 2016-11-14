using System;
using System.Collections.Generic;
using System.Windows.Documents;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.API;
using Xceed.Wpf.DataGrid.Converters;

namespace CourseworkTwoMetro.ViewModels
{
    public class MainWindowViewModel : FormWithSpinnerViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Booking> Bookings { get; set; }

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
                foreach (Customer i in Customers)
                {
                    Console.WriteLine(i.ReferenceNumber);
                    Console.WriteLine(i.Name);
                    Console.WriteLine(i.Address);
                }
                this.Bookings = await ApiFacade.GetBookings();
                foreach (Booking i in Bookings)
                {
                    Console.WriteLine(i.Id);
                    Console.WriteLine(i.ArrivalDate);
                    Console.WriteLine(i.DepartureDate);
                }
                this.LoadingFailed = false;
            }
            catch
            {
                this.LoadingFailed = true;
            }
            this.Loading = true;
        }
    }
}