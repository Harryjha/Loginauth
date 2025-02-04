using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CRUD_Dapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDestinationRepository _destinationRepository;

        public HomeController(ILogger<HomeController> logger, IDestinationRepository destinationRepository)
        {
            _logger = logger;
            _destinationRepository = destinationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ? Show all destinations (accessible to all users)
        public async Task<IActionResult> Destinations()
        {
            var destinations = await _destinationRepository.GetAllDestinations();
            return View(destinations);
        }

        // ? Show owner-specific destinations
        public async Task<IActionResult> MyDestinations()
        {
            int userId = GetUserId();
            var destinations = await _destinationRepository.GetDestinationsByOwner(userId);
            return View(destinations);
        }

        // ? Show Create Destination Form
        public IActionResult CreateDestination()
        {
            return View();
        }

        // ? Create a new destination (assigning owner)
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

        // ? Edit a destination (GET)
        public async Task<IActionResult> EditDestination(int id)
        {
            int userId = GetUserId();
            var destinations = await _destinationRepository.GetDestinationsByOwner(userId);
            var destination = destinations.FirstOrDefault(d => d.Id == id);

            if (destination == null)
            {
                return Unauthorized();  // User cannot edit someone else's destination
            }

            return View(destination);
        }

        // ? Update a destination (POST)
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
                return Unauthorized();  // User is not authorized to update
            }

            return RedirectToAction("MyDestinations");
        }

        // ? Delete a destination (only if owned by the user)
        [HttpPost]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            int userId = GetUserId();
            var deletedRows = await _destinationRepository.DeleteDestination(id, userId);

            if (deletedRows == 0)
            {
                return Unauthorized();  // Prevent unauthorized deletion
            }

            return RedirectToAction("MyDestinations");
        }

        // ?? Helper: Get the current user ID from session
        private int GetUserId()
        {
            return int.TryParse(HttpContext.Session.GetString("UserId"), out int userId) ? userId : 0;
        }
    }
}
