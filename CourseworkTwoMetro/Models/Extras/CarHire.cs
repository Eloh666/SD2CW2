using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class CarHire : Extra
    {
        private DateTime _hireStart;
        private DateTime _hireEnd;

        public CarHire() : base("CarHire")
        {
            HireStart = DateTime.Today;
            HireEnd = DateTime.Today.AddDays(1);
            this.NightlyCost = 50;
        }

        public CarHire(DateTime hireStart, DateTime hireEnd) : base("CarHire")
        {
            this.HireStart = hireStart;
            this.HireEnd = hireEnd;
        }

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

        public string StartDate => _hireStart.Date.ToString("d");

        public string EndDate => _hireEnd.Date.ToString("d");

        public override double GetCost(double nights, int guests)
        {
            return this.GetCost();
        }

        private double GetCost()
        {
            return NightlyCost * (this.HireEnd - this.HireStart).TotalDays;
        }
    }
}