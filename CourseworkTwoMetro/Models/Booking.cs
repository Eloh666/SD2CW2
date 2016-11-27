using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Extras;

namespace CourseworkTwoMetro.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }

        public string DietaryReqs { get; set; }

        public int CustomerId { get; set; }
        public List<Guest> Guests { get; set; }
        public Dictionary<string, Extra> Extra { get; }

        public Booking()
        {
            this.Extra = new Dictionary<string, Extra> {{"Breakfast", null}, {"Dinner", null}, {"CarHire", null}};
            this.Guests = new List<Guest>();
        }

        public Booking(int id, DateTime arrivalDate, DateTime departureDate, int customerId) : base()
        {
            this.Id = id;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.CustomerId = customerId;
        }

        public Dinner Dinner
        {
            get { return (Dinner)this.Extra["Dinner"]; }
            set { this.Extra["Dinner"] = value; }
        }

        public CarHire CarHire
        {
            get { return (CarHire)this.Extra["CarHire"]; }
            set { this.Extra["CarHire"] = value; }
        }

        public Breakfast Breakfast
        {
            get { return (Breakfast)this.Extra["Breakfast"]; }
            set { this.Extra["Breakfast"] = value; }
        }

        public double GetCost
        {
            get
            {
                double nights = (this.DepartureDate - this.ArrivalDate).TotalDays;
                double extras = this.Extra.Sum(entry => entry.Value?.GetCost(nights) ?? 0);
                double bookings = Guests.Sum(guest => (guest.Age < 18 ? 30 : 50) * nights);
                return extras + bookings;
            }
        }

        public string ValidateArrivalDate()
        {
            if (this.ArrivalDate < default(DateTime))
            {
                return "Your booking date cannot start in the past";
            }
            if (this.ArrivalDate > this.DepartureDate)
            {
                return "Your booking cannot start after its end";
            }

            return null;
        }

        public string ValidateDepartureDate()
        {
            if (this.DepartureDate < DateTime.Now)
            {
                return "Your booking date cannot start in the past";
            }
            if (this.ArrivalDate > this.DepartureDate)
            {
                return "Your booking cannot end before its start";
            }
            return null;
        }

        public string ValidateGuestsList()
        {
            return this.Guests.Count == 0 ? "You need at leaast a guest for this booking" : null;
        }

    }
}