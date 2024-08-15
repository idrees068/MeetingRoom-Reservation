using MeetingRoomReservation.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsers _userRepository;

        public AuthController(IUsers userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(email, password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.ID);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
