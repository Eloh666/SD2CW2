namespace CourseworkTwoMetro.Models.Extras
{
    public abstract class Extra
    {
        private const double NightlyCost = 0;
        private string type;

        protected Extra(string type)
        {
            this.type = type;
        }

        public virtual double GetCost(double nights)
        {
            return NightlyCost * nights;
        }
    }
}