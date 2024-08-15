using MeetingRoomReservation.Models;

namespace MeetingRoomReservation.Repository.Interface
{
    public interface IUsers
    {

        public UsersModel GetUserByEmailAndPassword(string email, string password);
        public bool RegisterUser(UsersModel user);
        public  UsersModel GetUserById(int userId);
        public List<UsersModel> GetAllUsers();

    }
}
