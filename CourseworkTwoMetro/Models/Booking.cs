using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CourseworkTwoMetro.Models.Extras;

namespace CourseworkTwoMetro.Models
{
    public class Booking
    {
        public string Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }

        public string DietaryReqs { get; set; }

        public string CustomerId { get; set; }
        public List<Guest> Guests { get; set; }
        public Dictionary<string, Extra> Extras { get; set; }

        public Booking()
        {
            this.Extras = new Dictionary<string, Extra> {{"Breakfast", null}, {"Dinner", null}, {"CarHire", null}};
            this.Guests = new List<Guest>();
        }

        public Booking(string id, DateTime arrivalDate, DateTime departureDate, string customerId) : base()
        {
            this.Id = id;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.CustomerId = customerId;
        }

        public double GetCost
        {
            get
            {
                double nights = (this.ArrivalDate - this.DepartureDate).TotalDays;
                double extras = Extras.Sum(entry => entry.Value?.GetCost(nights) ?? 0);
                double bookings = Guests.Sum(guest => (guest.Age < 18 ? 30 : 50) * nights);
                return extras + bookings;
            }
        }

    }
}