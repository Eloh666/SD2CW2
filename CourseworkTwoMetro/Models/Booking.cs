﻿using System;
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
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// The main booking object that encompasses its extras,
    /// guests and some validation methods
    /// </summary>
    [Serializable]
    public class Booking : ClonableModel
    {
        private DateTime _arrivalDate;
        private DateTime _departureDate;
        public int Id { get; set; }

        public string DietaryReqs { get; set; }

        public int CustomerId { get; set; }
        public ObservableCollection<Guest> Guests { get; set; }
        public Dictionary<string, Extra> Extras { get; }

        // 0 params constructor used when creating a new booking
        public Booking()
        {
            this.Extras = new Dictionary<string, Extra> {{"Breakfast", null}, {"Dinner", null}, {"CarHire", null}};
            this.Guests = new ObservableCollection<Guest>();
            this.ArrivalDate = DateTime.Today;
            this.DepartureDate = DateTime.Today.AddDays(1);
        }

        // full params constructor used when generating from serialization
        public Booking(int id, DateTime arrivalDate, DateTime departureDate, int customerId) : base()
        {
            this.Id = id;
            this.ArrivalDate = arrivalDate;
            this.DepartureDate = departureDate;
            this.CustomerId = customerId;
        }

        // getters and setters
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

        // returns a short date from the datetime object
        public string StartDate => _arrivalDate.Date.ToString("d");

        // returns a short date from the datetime object
        public string EndDate => _departureDate.Date.ToString("d");

        // returns the cost of the booking
        public double GetCost
        {
            get
            {
                // duration of the booking
                double nights = (this.DepartureDate - this.ArrivalDate).TotalDays;
                // cost of the extras
                double extras = this.Extras.Sum(entry => entry.Value?.GetCost(nights, Guests.Count) ?? 0);
                // cost of the booking itself
                double bookings = Guests.Sum(guest => (guest.Age < 18 ? 30 : 50) * nights);
                return extras + bookings;
            }
        }

        // validation for the booking nights... in retrospect I kinda went a bit crazy with ternaries here
        public string ValidateArrivalDate() => this.ArrivalDate < DateTime.Today
            ? "Your booking date cannot start in the past"
            : (this.ArrivalDate > this.DepartureDate
                ? "Your booking cannot start after its end"
                : (this.ArrivalDate == DepartureDate ? "Your booking must last at least one day." : null));

        public string ValidateDepartureDate() => this.DepartureDate < DateTime.Today.AddDays(1)
            ? "Your booking date cannot start in the past"
            : (this.ArrivalDate > this.DepartureDate
                ? "Your booking cannot end before its start"
                : (this.ArrivalDate == DepartureDate ? "Your booking must last at least one day." : null));

        // and validation for the booking guests
        public string ValidateGuestsList()
        {
            return this.Guests.Count == 0
                ? "You need at least one guest to proceed with your booking."
                : (this.Guests.Count > 4 ? "Too many guests selected." : null);
        }

    }
}