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

        public string CustomerId { get; set; }
        public List<Guest> Guests { get; set; }
        public List<Extra> Extras { get; set; }

        public Booking(string id, DateTime arrivalDate, DateTime departureDate, string customerId)
        {
            this.Id = id;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.CustomerId = customerId;
        }

    }
}