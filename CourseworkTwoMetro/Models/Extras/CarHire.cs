using System;

namespace CourseworkTwoMetro.Models.Extras
{
    public class CarHire : Extra
    {
        private const double NightlyCost = 50;
        public DateTime HireStart { get; set; }
        public DateTime HireEnd { get; set; }

        public CarHire() : base("CarHire")
        {
        }

        public override double GetCost(double nights)
        {
            return this.GetCost();
        }

        private double GetCost()
        {
            return NightlyCost * (this.HireEnd - this.HireStart).TotalDays;
        }
    }
}