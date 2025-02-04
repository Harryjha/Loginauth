using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRUD_Dapper.Controllers
{
    public class DestinationController : Controller
    {
        private readonly IDestinationRepository _destinationRepository;

        public DestinationController(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        // ✅ List destinations for the current owner
        public async Task<IActionResult> Index()
        {
            int ownerId = int.Parse(HttpContext.Session.GetString("UserId")); // Get logged-in user's ID
            var destinations = await _destinationRepository.GetDestinationsByOwner(ownerId);
            return View(destinations);
        }

        // ✅ Create Destination (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Create Destination (POST)
        [HttpPost]
        public async Task<IActionResult> CreateDestination(Destination destination)
        {
            // Always set the OwnerId from the session for security
            destination.OwnerId = int.Parse(HttpContext.Session.GetString("UserId"));

            await _destinationRepository.AddDestination(destination);
            return RedirectToAction("Index");
        }


        // ✅ Edit Destination (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int ownerId = int.Parse(HttpContext.Session.GetString("UserId"));
            var destinations = await _destinationRepository.GetDestinationsByOwner(ownerId);
            var destination = destinations.FirstOrDefault(d => d.Id == id);

            if (destination == null)
                return NotFound();  // Destination not found or doesn't belong to the owner

            return View(destination);
        }

        // ✅ Edit Destination (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Destination destination)
        {
            int ownerId = int.Parse(HttpContext.Session.GetString("UserId"));
            await _destinationRepository.UpdateDestination(destination, ownerId);
            return RedirectToAction("Index");
        }

        // ✅ Delete Destination
        public async Task<IActionResult> Delete(int id)
        {
            int ownerId = int.Parse(HttpContext.Session.GetString("UserId"));
            await _destinationRepository.DeleteDestination(id, ownerId);
            return RedirectToAction("Index");
        }
    }
}
