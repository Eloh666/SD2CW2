using System;

namespace CourseworkTwoMetro.Models.Extras
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Serializible parent/abastract class
    /// used by the factory pattern
    /// </summary>
    [Serializable]
    public abstract class Extra
    {
        protected double NightlyCost = 0;
        private string _type;

        protected Extra(string type)
        {
            this._type = type;
        }

        // dummy virtual method that returns the cost of the extra given the nights
        public virtual double GetCost(double nights, int guests)
        {
            return NightlyCost * nights * guests;
        }
    }
}