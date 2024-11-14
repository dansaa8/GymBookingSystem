namespace GymBookingSystem.Models
{
    // Kopplingstabell mellan ApplicationUser och GymClass
    public class ApplicationUserGymClass
    {
        public string ApplicationUserId { get; set; }
        public int GymClassId { get; set; }
    }
}
