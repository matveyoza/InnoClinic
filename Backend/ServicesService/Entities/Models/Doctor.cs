namespace Entities.Models
{
    public class DoctorProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public int AppointmentDuration { get; set; }
    }
}
