using MeetingRoomReservation.Models;
using MeetingRoomReservation.Repository.Implementation;

namespace MeetingRoomReservation.Repository.Interface
{
    public interface IReservation
    {
      public  List<ReservationModel> GetReservationList();
      public List<ReservationModel> GetReservationsByUserId(int userId);
      public bool AddReservation(ReservationModel reservation);
      public bool EndReservation(int reservationId);
    }
}
