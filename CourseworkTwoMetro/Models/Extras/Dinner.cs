using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class Dinner : Extra
    {
        public Dinner() : base("Dinner")
        {
        }

        private const double DinnerCost = 15;
    }
}