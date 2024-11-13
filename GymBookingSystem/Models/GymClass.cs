namespace GymBookingSystem.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } } // only get prop, will not be saved to GymClass table as a col
        public string Description { get; set; }

        // Navigation prop:
        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; } = new List<ApplicationUserGymClass>();
    }
}
