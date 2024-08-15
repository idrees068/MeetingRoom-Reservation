namespace MeetingRoomReservation.Models
{
    public class ReservationModel
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public UsersModel User { get; set; }
    }
}
