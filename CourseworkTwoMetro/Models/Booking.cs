using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CourseworkOneMetro.Models.Utils;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    [Serializable]
    public class Booking : ICloneable
    {
        private DateTime _arrivalDate;
        private DateTime _departureDate;
        public int Id { get; set; }

        public string DietaryReqs { get; set; }

        public int CustomerId { get; set; }
        public ObservableCollection<Guest> Guests { get; set; }
        public Dictionary<string, Extra> Extras { get; }

        public Booking()
        {
            this.Extras = new Dictionary<string, Extra> {{"Breakfast", null}, {"Dinner", null}, {"CarHire", null}};
            this.Guests = new ObservableCollection<Guest>();
            this.ArrivalDate = DateTime.Today;
            this.DepartureDate = DateTime.Today.AddDays(1);
        }

        public Booking(int id, DateTime arrivalDate, DateTime departureDate, int customerId) : base()
        {
            this.Id = id;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.CustomerId = customerId;
        }

        public DateTime ArrivalDate
        {
            get { return _arrivalDate.Date; }
            set { _arrivalDate = value; }
        }

        public DateTime DepartureDate
        {
            get { return _departureDate.Date; }
            set { _departureDate = value; }
        }

        public string StartDate => _arrivalDate.Date.ToString("d");

        public string EndDate => _departureDate.Date.ToString("d");


        public Dinner Dinner
        {
            get { return (Dinner)this.Extras["Dinner"]; }
            set { this.Extras["Dinner"] = value; }
        }

        public CarHire CarHire
        {
            get { return (CarHire)this.Extras["CarHire"]; }
            set { this.Extras["CarHire"] = value; }
        }

        public Breakfast Breakfast
        {
            get { return (Breakfast)this.Extras["Breakfast"]; }
            set { this.Extras["Breakfast"] = value; }
        }

        public double GetCost
        {
            get
            {
                double nights = (this.DepartureDate - this.ArrivalDate).TotalDays;
                double extras = this.Extras.Sum(entry => entry.Value?.GetCost(nights, Guests.Count) ?? 0);
                double bookings = Guests.Sum(guest => (guest.Age < 18 ? 30 : 50) * nights);
                return extras + bookings;
            }
        }

        public string ValidateArrivalDate() => this.ArrivalDate < DateTime.Today
            ? "Your booking date cannot start in the past"
            : (this.ArrivalDate > this.DepartureDate
                ? "Your booking cannot start after its end"
                : (this.ArrivalDate == DepartureDate ? "Your booking must last at least one day." : null));

        public string ValidateDepartureDate() => this.DepartureDate < DateTime.Today
            ? "Your booking date cannot start in the past"
            : (this.ArrivalDate > this.DepartureDate
                ? "Your booking cannot end before its start"
                : (this.ArrivalDate == DepartureDate ? "Your booking must last at least one day." : null));

        public string ValidateGuestsList()
        {
            return this.Guests.Count == 0 ? "You need at leaast a guest for this booking" : null;
        }

        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }

    }
}