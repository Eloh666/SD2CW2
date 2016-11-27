namespace CourseworkTwoMetro.Models.Extras
{
    public class ExtrasFactory
    {
        public static Extra CreateExtra(string extraType)
        {
            switch (extraType)
            {
                case "Breakfast":
                    return new Breakfast();
                case "Dinner":
                    return new Dinner();
                case "CarHire":
                    return new CarHire();
                default:
                    return null;
            }
        }
    }
}