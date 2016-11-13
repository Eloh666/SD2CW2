using CourseworkOneMetro.Models.Utils;

namespace CourseworkTwoMetro.Models
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

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