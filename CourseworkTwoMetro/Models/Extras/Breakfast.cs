using System;
using CourseworkTwoMetro.Models.Utils;

namespace CourseworkTwoMetro.Models.Extras
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Breakfast extra class, inherits from extras.
    /// Just a convenient dummy object used for serialization
    /// </summary>
    [Serializable]
    public class Breakfast : Extra, ICloneable
    {
        public Breakfast() : base("Breakfast")
        {
            this.NightlyCost = 5;
        }

        // simple implementation of the clonable interface
        public object Clone()
        {
            return CloneUtils.DeepClone(this);
        }
    }
}