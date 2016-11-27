using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class CarHire : Extra
    {
        private const double NightlyCost = 50;
        public DateTime HireStart { get; set; }
        public DateTime HireEnd { get; set; }

        public CarHire() : base("CarHire")
        {
        }

        public CarHire(DateTime hireStart, DateTime hireEnd) : base("CarHire")
        {
            this.HireStart = hireStart;
            this.HireEnd = hireEnd;
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