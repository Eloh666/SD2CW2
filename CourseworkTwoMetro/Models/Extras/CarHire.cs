using System;

namespace CourseworkTwoMetro.Models.Extras
{
    public class CarHire : Extra
    {
        public DateTime HireStart { get; set; }
        public DateTime HireEnd { get; set; }
        public CarHire(double nightlyCost) : base(50)
        {
        }

        public override double GetCost(int nights)
        {
            return this.GetCost();
        }

        public double GetCost()
        {
            return this.NightlyCost * (HireEnd - HireStart).TotalDays;
        }
    }
}