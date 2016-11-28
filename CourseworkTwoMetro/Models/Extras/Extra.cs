using System;

namespace CourseworkTwoMetro.Models.Extras
{
    [Serializable]
    public abstract class Extra
    {
        protected double NightlyCost = 0;
        private string _type;

        protected Extra(string type)
        {
            this._type = type;
        }

        public virtual double GetCost(double nights, int guests)
        {
            return NightlyCost * nights * guests;
        }
    }
}