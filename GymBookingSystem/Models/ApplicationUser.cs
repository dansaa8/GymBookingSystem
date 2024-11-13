using Microsoft.AspNetCore.Identity;

namespace GymBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation prop:
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
