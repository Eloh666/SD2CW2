using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.ViewModels.Utils;

namespace CourseworkTwoMetro.ViewModels
{
    public class BookingViewModel : PropertyChangedNotifier, IDataErrorInfo
    {
        private readonly Dictionary<string, bool> _fieldsUseDictionary;
        private Booking _booking;

        public BookingViewModel(Booking booking)
        {
            this.Booking = booking;
            _fieldsUseDictionary = new Dictionary<string, bool>();
            this._fieldsUseDictionary.Add("Id", false);
            this._fieldsUseDictionary.Add("ArrivalDate", false);
            this._fieldsUseDictionary.Add("DepartureDate", false);
            this._fieldsUseDictionary.Add("DietaryReqs", false);
            this._fieldsUseDictionary.Add("Dinner", false);
            this._fieldsUseDictionary.Add("CarHire", false);
            this._fieldsUseDictionary.Add("Breakfast", false);
            this._fieldsUseDictionary.Add("Guests", false);
        }

        public Booking Booking
        {
            get { return _booking; }
            set
            {
                _booking = value;
                OnPropertyChangedEvent(null);
            }
        }

        public int Id
        {
            get { return Booking.Id; }
            set
            {
                this.Booking.Id = value;
                OnPropertyChangedEvent("Id");
            }
        }

        public DateTime ArrivalDate
        {
            get { return Booking.ArrivalDate; }
            set
            {
                this.Booking.ArrivalDate = value;
                this._fieldsUseDictionary["ArrivalDate"] = true;
                OnPropertyChangedEvent("ArrivalDate");
            }
        }

        public DateTime DepartureDate
        {
            get { return Booking.DepartureDate; }
            set
            {
                this.Booking.DepartureDate = value;
                this._fieldsUseDictionary["DepartureDate"] = true;
                OnPropertyChangedEvent("DepartureDate");
            }
        }

        public string DietaryReqs
        {
            get { return Booking.DietaryReqs; }
            set
            {
                this.Booking.DietaryReqs = value;
                this._fieldsUseDictionary["DietaryReqs"] = true;
                OnPropertyChangedEvent("DietaryReqs");
            }
        }

        public List<Guest> Guests
        {
            get { return Booking.Guests; }
            set
            {
                this.Booking.Guests = value;
                this._fieldsUseDictionary["Guests"] = true;
                OnPropertyChangedEvent("Guests");
            }
        }

        public Dinner Dinner
        {
            get { return Booking.Dinner; }
            set
            {
                this.Booking.Dinner = value;
                this._fieldsUseDictionary["Dinner"] = true;
                OnPropertyChangedEvent("Dinner");
            }
        }

        public CarHire CarHire
        {
            get { return Booking.CarHire; }
            set
            {
                this.Booking.CarHire = value;
                this._fieldsUseDictionary["CarHire"] = true;
                OnPropertyChangedEvent("CarHire");
            }
        }

        public Breakfast Breakfast
        {
            get { return Booking.Breakfast; }
            set
            {
                this.Booking.Breakfast = value;
                this._fieldsUseDictionary["Breakfast"] = true;
                OnPropertyChangedEvent("Breakfast");
            }
        }

        public double GetCost => Booking.GetCost;

        public bool IsBreakfastSelected => this.Booking.Breakfast != null;
        public bool IsDinnerSelected => this.Booking.Dinner != null;
        public bool IsCarHireSelected => this.Booking.CarHire != null;

        public string IdString => "Booking ID: " + this.Id;
        public string ArrivalDateString => "Arrival Date: " + this.ArrivalDate;
        public string DepartureDateString => "Departure Date: " + this.DepartureDate;

        public string DietaryReqsString => string.IsNullOrEmpty(this.DietaryReqs) ? "Dietary Requirements: None" : "Dietary Requirements: " + this.DietaryReqs;

        public string GuestsNumberString => "Number of guests: " + this.Guests.Count;

        public string HireStartString => "Hire Start: " + CarHire?.HireStart;
        public string HireEndString => "Hire End: " + CarHire?.HireEnd;

        public string GetCostString => "Total due: £" + Booking.GetCost;

        string IDataErrorInfo.Error => null;
        string IDataErrorInfo.this[string fieldName] => GetValidationError(fieldName);

        // fields that require validation
        private static readonly string[] ValidationFields =
        {
            "ArrivalDate",
            "DepartureDate",
            "Guests",
        };

        public bool IsBookingValid
        {
            get
            {
                this._fieldsUseDictionary["ArrivalDate"] = true;
                this._fieldsUseDictionary["DepartureDate"] = true;

                if (ValidationFields.Any(field => GetValidationError(field) != null))
                {
                    OnPropertyChangedEvent(null);
                    return false;
                }
                OnPropertyChangedEvent(null);
                return true;
            }
        }

        // validates fields based on the requirements of the model
        private string GetValidationError(string fieldName)
        {
            string error = null;
            if (this._fieldsUseDictionary[fieldName])
            {
                switch (fieldName)
                {
                    case "ArrivalDate":
                        error = this.Booking.ValidateArrivalDate();
                        break;
                    case "DepartureDate":
                        error = this.Booking.ValidateDepartureDate();
                        break;
                    case "Guests":
                        error = this.Booking.ValidateGuestsList();
                        break;
                }
            }
            return error;
        }


    }
}