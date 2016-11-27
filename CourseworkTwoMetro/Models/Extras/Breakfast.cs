using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class Breakfast : Extra
    {
        private const double BreakfastCost = 5;

        public Breakfast() : base("Breakfast")
        {
        }
    }
}