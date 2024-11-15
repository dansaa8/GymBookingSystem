using Microsoft.AspNetCore.Identity;

namespace GymBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; // Shadow prop
        public DateTime TimeOfRegistration { get; set; }
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; } = new List<ApplicationUserGymClass>(); // Navigation prop
    }
}
