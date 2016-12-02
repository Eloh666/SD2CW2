namespace CourseworkTwoMetro.Models.Extras
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Uses the design pattern and returns a new extra based on a string keyword
    /// </summary>
    public class ExtrasFactory
    {
        // static factory method that returns the object instance based on the given string
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