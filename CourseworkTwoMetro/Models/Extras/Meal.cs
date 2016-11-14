namespace CourseworkTwoMetro.Models.Extras
{
    public abstract class Meal : Extra
    {
        public string DietaryReqs { get; set; }
        protected Meal(double nightlyCost, string dietaryReqs) : base(nightlyCost)
        {
            this.DietaryReqs = dietaryReqs;
        }
    }
}