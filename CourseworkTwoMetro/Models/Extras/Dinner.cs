using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class Dinner : Extra
    {
        public Dinner() : base("Dinner")
        {
            this.NightlyCost = 15;
        }

        private const double DinnerCost = 15;
    }
}