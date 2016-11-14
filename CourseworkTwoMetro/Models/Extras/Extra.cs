namespace CourseworkTwoMetro.Models.Extras
{
    public abstract class Extra
    {
        public double NightlyCost { get; set; }

        protected Extra(double nightlyCost)
        {
            NightlyCost = nightlyCost;
        }

        public virtual double GetCost(int nights)
        {
            return NightlyCost * nights;
        }
    }
}