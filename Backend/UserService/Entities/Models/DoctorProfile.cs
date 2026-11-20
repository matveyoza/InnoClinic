namespace Entities.Models
{
    public class DoctorProfile
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public string Specialty { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
    }
}
