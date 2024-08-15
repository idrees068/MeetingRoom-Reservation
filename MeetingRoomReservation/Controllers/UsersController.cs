using MeetingRoomReservation.Models;
using MeetingRoomReservation.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsers _userRepository;

        public UsersController(IUsers userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UsersModel user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.RegisterUser(user);
                return RedirectToAction("Index", "Auth");
            }

            return View(user);
        }
    }
}
