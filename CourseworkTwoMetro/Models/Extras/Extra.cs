using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public abstract class Extra
    {
        private const double NightlyCost = 0;
        private string _type;

        public Extra(string type)
        {
            this._type = type;
        }

        public virtual double GetCost(double nights)
        {
            return NightlyCost * nights;
        }
    }
}