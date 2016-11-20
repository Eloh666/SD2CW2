namespace CourseworkTwoMetro.Models.Extras
{
    public abstract class Extra
    {
        private const double NightlyCost = 0;

        public virtual double GetCost(double nights)
        {
            return NightlyCost * nights;
        }
    }
}