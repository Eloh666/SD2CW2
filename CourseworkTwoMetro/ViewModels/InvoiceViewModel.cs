using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Navigation;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class InvoiceViewModel
    {
        // managers singleton
        public CommandsManager Commands { get; }
        public DateTime CurrentDate { get; }
        public BookingViewModel BookingToDisplay { get; }
        public string InvoiceText { get; }
        public InvoiceViewModel(MainViewModel mainViewModel, BookingViewModel bookingToDisplay, ObservableCollection<Customer> customers)
        {
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.CurrentDate = DateTime.Now;
            this.BookingToDisplay = bookingToDisplay;
            string customerName = customers.Where(customer => customer.ReferenceNumber == bookingToDisplay.CustomerId).ToString();
            this.InvoiceText = this.initializeInvoiceText(bookingToDisplay, customerName);
        }

        // creates an invoice based on the data of the booking
        private string initializeInvoiceText(BookingViewModel bookingToDisplay, string customerName)
        {
            List<string> invoiceDetails = new List<string>();
            int stay = (bookingToDisplay.DepartureDate - bookingToDisplay.ArrivalDate).Days;
            invoiceDetails.Add("Dear " + customerName);
            invoiceDetails.Add("Thanks for choosing Napier's Holiday Village. You will find here the invoice for your booking.\n");
            invoiceDetails.Add("The duration of your stay at the resort is of " + stay + " days.");
            invoiceDetails.Add("\nThe guests for your booking will be: \n");
            foreach (Guest guest in bookingToDisplay.Guests)
            {
                invoiceDetails.Add(guest.Name);
            }
            invoiceDetails.Add("\nYou have also selected the following extras:");
            if (bookingToDisplay.Breakfast != null)
            {
                invoiceDetails.Add("Breakfast, ");
            }
            if (bookingToDisplay.Dinner != null)
            {
                invoiceDetails.Add("Dinner, ");
            }
            if (!string.IsNullOrEmpty(bookingToDisplay.DietaryReqsString))
            {
                invoiceDetails.Add("\nand you have chosen the following dietary requirements: " + bookingToDisplay.DietaryReqsString);
            }
            if (bookingToDisplay.CarHire != null)
            {
                invoiceDetails.Add("\n\nYou have chosen to hire a car for the period between, " + bookingToDisplay.CarHireStart.Date.ToString("d") + " and " + bookingToDisplay.CarHireEnd.Date.ToString("d"));
            }
            return string.Join("\n", invoiceDetails);
        }
    }
}