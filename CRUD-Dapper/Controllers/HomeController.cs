using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Dapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDestinationRepository _destinationRepository;
        private readonly IBookingRepository _bookingRepository;

        public HomeController(ILogger<HomeController> logger, IDestinationRepository destinationRepository, IBookingRepository bookingRepository)
        {
            _logger = logger;
            _destinationRepository = destinationRepository;
            _bookingRepository = bookingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Show all destinations
        public async Task<IActionResult> Destinations()
        {
            var destinations = await _destinationRepository.GetAllDestinations();
            return View(destinations);
        }

        // Owner-specific destinations
        public async Task<IActionResult> MyDestinations()
        {
            int userId = GetUserId();
            var destinations = await _destinationRepository.GetDestinationsByOwner(userId);
            return View(destinations);
        }

        // Create Destination (GET)
        public IActionResult CreateDestination()
        {
            return View();
        }

        // Create Destination (POST)
        [HttpPost]
        public async Task<IActionResult> CreateDestination(Destination model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.OwnerId = GetUserId();
            model.CreatedDate = DateTime.Now;

            await _destinationRepository.AddDestination(model);
            return RedirectToAction("MyDestinations");
        }

        // Edit Destination (GET)
        public async Task<IActionResult> EditDestination(int id)
        {
            int userId = GetUserId();
            var destinations = await _destinationRepository.GetDestinationsByOwner(userId);
            var destination = destinations.FirstOrDefault(d => d.Id == id);

            if (destination == null)
            {
                return Unauthorized();
            }

            return View(destination);
        }

        // Edit Destination (POST)
        [HttpPost]
        public async Task<IActionResult> EditDestination(Destination model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = GetUserId();
            var result = await _destinationRepository.UpdateDestination(model, userId);

            if (result == 0)
            {
                return Unauthorized();
            }

            return RedirectToAction("MyDestinations");
        }

        // Delete Destination
        [HttpPost]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            int userId = GetUserId();
            var deletedRows = await _destinationRepository.DeleteDestination(id, userId);

            if (deletedRows == 0)
            {
                return Unauthorized();
            }

            return RedirectToAction("MyDestinations");
        }

        // ✅ Book Destination (GET)
        public IActionResult BookDestination(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.DestinationId = id;
            return View();
        }

        // ✅ Book Destination (POST)
        [HttpPost]
        public async Task<IActionResult> BookDestination(int destinationId, DateTime bookingDate)
        {
            int userId = GetUserId();
            var booking = new Booking
            {
                UserId = userId,
                DestinationId = destinationId,
                BookingDate = bookingDate
            };

            await _bookingRepository.AddBooking(booking);
            return RedirectToAction("MyBookings");
        }

        // ✅ View Bookings
        public async Task<IActionResult> MyBookings()
        {
            int userId = GetUserId();
            var bookings = await _bookingRepository.GetUserBookings(userId);
            return View(bookings);
        }

        // ✅ Cancel Booking
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            int userId = GetUserId();
            await _bookingRepository.CancelBooking(bookingId, userId);
            return RedirectToAction("MyBookings");
        }

        // Helper: Get current user ID
        private int GetUserId()
        {
            return int.TryParse(HttpContext.Session.GetString("UserId"), out int userId) ? userId : 0;
        }
    }
}
