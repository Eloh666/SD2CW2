using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public class Breakfast : Extra
    {
        public Breakfast() : base("Breakfast")
        {
            this.NightlyCost = 5;
        }
    }
}