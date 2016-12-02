using System;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models.Extras
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Car hire object, inherits from extras
    /// very handy for serialization
    /// </summary>
    [Serializable]
    public class CarHire : Extra, ICloneable
    {
        private DateTime _hireStart;
        private DateTime _hireEnd;

        // 0 params constructor, used when creating
        // sets the start date to today and the previous one to tomorrow
        public CarHire() : base("CarHire")
        {
            HireStart = DateTime.Today;
            HireEnd = DateTime.Today.AddDays(1);
            this.NightlyCost = 50;
        }

        // 2 params constructor, used when creating the object from JSON input
        public CarHire(DateTime hireStart, DateTime hireEnd) : base("CarHire")
        {
            this.HireStart = hireStart;
            this.HireEnd = hireEnd;
        }

        // getters and setters
        public DateTime HireStart
        {
            get { return _hireStart.Date; }
            set { _hireStart = value; }
        }

        public DateTime HireEnd
        {
            get { return _hireEnd.Date; }
            set { _hireEnd = value; }
        }

        // returns a short date from the datetime object
        public string StartDate => _hireStart.Date.ToString("d");

        // returns a short date from the datetime object
        public string EndDate => _hireEnd.Date.ToString("d");


        // override of the base method that calls a specific get cost
        public override double GetCost(double nights, int guests)
        {
            return this.GetCost();
        }

        // returns the extra cost based on the amount of nights
        private double GetCost()
        {
            return NightlyCost * (this.HireEnd - this.HireStart).TotalDays;
        }

        // simple implementation of the clonable interface
        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }
    }
}