
using MeetingRoomReservation.Models;
using MeetingRoomReservation.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IReservation _reservationRepo;
    private readonly IUsers _userRepository;

    public HomeController(IReservation reservationRepository, IUsers userRepository)
    {
        _reservationRepo = reservationRepository;
        _userRepository = userRepository;
    }

    // Dashboard displaying all reservations
    public ActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        // Fetch all reservations
        var allReservations = _reservationRepo.GetReservationList();

        // Filter to only include active reservations
        var activeReservations = allReservations.Where(r => r.IsActive).ToList();

        // Pass the filtered list to the view
        var viewModel = new ViewModel
        {
            Reservations = activeReservations
        };

        var user = _userRepository.GetUserById(userId.Value);

        if (user != null)
        {
            ViewBag.UserName = user.Name;
            ViewBag.UserId = user.ID;
        }
        else
        {
            ViewBag.UserName = "Guest";
            ViewBag.UserId = 0; // Set to 0 or a default value
        }

        return View(viewModel);
    }

    // My Reservations displaying reservations for the logged-in user
    public ActionResult Reservations()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var viewModel = new ViewModel
        {
            Reservations = _reservationRepo.GetReservationsByUserId(userId.Value),
            UserList = new List<UsersModel> { _userRepository.GetUserById(userId.Value) }
        };

        var user = _userRepository.GetUserById(userId.Value);

        if (user != null)
        {
            ViewBag.UserName = user.Name;
            ViewBag.UserId = user.ID;
        }

        return View(viewModel);
    }

    /*    [HttpPost]
        public ActionResult ReserveRoom(int userId, DateTime startTime, DateTime endTime)
        {
            var reservations = _reservationRepo.GetReservationList();
            var conflictingReservation = reservations.FirstOrDefault(r => r.IsActive && ((startTime >= r.StartTime && startTime < r.EndTime) || (endTime > r.StartTime && endTime <= r.EndTime)));

            if (conflictingReservation != null)
            {
                ViewBag.ErrorMessage = $"The meeting room is already reserved until {conflictingReservation.EndTime}.";
                var user = _userRepository.GetUserById(userId);
                ViewBag.UserName = user?.Name ?? "Guest";
                ViewBag.UserId = userId;
                return View();
            }

            ReservationModel newReservation = new ReservationModel
            {
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                IsActive = true
            };

            _reservationRepo.AddReservation(newReservation);

            return RedirectToAction("Index");
        }
    */
    public ActionResult AddReservation()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _userRepository.GetUserById(userId.Value);

        if (user != null)
        {
            ViewBag.UserName = user.Name;
            ViewBag.UserId = user.ID;
        }
        else
        {
            ViewBag.UserName = "Guest";
            ViewBag.UserId = 0; // Set to 0 or a default value
        }

        return View();
    }

    // Action to handle the form submission for adding a reservation
    [HttpPost]
    public ActionResult AddReservation(int userId, DateTime startTime, DateTime endTime)
    {
        // Retrieve all existing reservations
        var reservations = _reservationRepo.GetReservationList();

        // Check for any conflicting reservation
        var conflictingReservation = reservations.FirstOrDefault(r => r.IsActive &&
            ((startTime >= r.StartTime && startTime < r.EndTime) ||
             (endTime > r.StartTime && endTime <= r.EndTime)));

        if (conflictingReservation != null)
        {
            // If a conflict is found, set the error message and re-display the form
            ViewBag.ErrorMessage = $"The meeting room is already reserved until {conflictingReservation.EndTime}.";

            // Retrieve user information
            var user = _userRepository.GetUserById(userId);

            // Ensure the user details are passed back to the view
            ViewBag.UserName = user?.Name ?? "Guest";
            ViewBag.UserId = userId;

            // Return the form view (ensure this returns the correct view, e.g., AddReservation)
            return View("AddReservation");
        }

        // If no conflict, create the new reservation
        ReservationModel newReservation = new ReservationModel
        {
            UserId = userId,
            StartTime = startTime,
            EndTime = endTime,
            IsActive = true
        };

        // Save the reservation to the database
        _reservationRepo.AddReservation(newReservation);

        // Redirect to the index or dashboard after successful reservation
        return RedirectToAction("Index");
    }



    [HttpPost]
    public ActionResult EndMeeting(int reservationId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        _reservationRepo.EndReservation(reservationId);
        return RedirectToAction("Reservations");
    }


    
}
