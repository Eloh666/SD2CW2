using CourseworkOneMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// Simple user class used for login purposes
    /// </summary>
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        // basic validation utility functions
        public string ValidatePassword()
        {
            return ValidationUtilities.ValidateNonEmpty("Password", this.Password);
        }

        public string ValidateUsername()
        {
            return ValidationUtilities.ValidateNonEmpty("Password", this.Username);
        }

    }
}