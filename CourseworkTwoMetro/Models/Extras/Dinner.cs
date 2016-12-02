using System;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models.Extras
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Dinner extra class, inherits from extras.
    /// Just a convenient dummy object used for serialization
    /// </summary>
    [Serializable]
    public class Dinner : Extra, ICloneable
    {
        public Dinner() : base("Dinner")
        {
            this.NightlyCost = 15;
        }

        // simple implementation of the clonable interface
        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }

        private const double DinnerCost = 15;
    }
}