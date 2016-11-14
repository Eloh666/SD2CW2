using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CourseworkTwoMetro.Models.Extras;

namespace CourseworkTwoMetro.Models
{
    public class Booking
    {
        public string Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }

        public Customer CurrentCustomer { get; set; }
        public List<Guest> Guests { get; set; }
        public List<Extra> Extras { get; set; }

        public Booking(string id, DateTime arrivalDate, DateTime departureDate, Customer currentCustomer)
        {
            Id = id;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            CurrentCustomer = currentCustomer;
        }

        public Booking(string id, DateTime arrivalDate, DateTime departureDate, string dietaryReqs, Customer currentCustomer, List<Guest> guests, List<Extra> extras)
        {
            Id = id;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            CurrentCustomer = currentCustomer;
            Guests = guests;
            Extras = extras;
        }
    }
}