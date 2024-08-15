namespace MeetingRoomReservation.Models
{
    public class ViewModel
    {
        public List<UsersModel> UserList { get; set; }
        public UsersModel User { get; set; }
        public List<ReservationModel> Reservations { get; set; }

        public ViewModel() 
        { 
        UserList = new List<UsersModel>();
        User = new UsersModel();
        Reservations = new List<ReservationModel>();
          
        }

    }
}
