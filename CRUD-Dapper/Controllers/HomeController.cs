using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        // ? GET: Show all destinations for customers
        public async Task<IActionResult> Destinations()
        {
            var destinations = await _destinationRepository.GetAllDestinations();
            return View(destinations);
        }

        // ? GET: Show Create Destination Form
        public IActionResult CreateDestination()
        {
            return View();
        }

        // ? POST: Create a new destination (assigning owner)
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
            return RedirectToAction("Destinations");
        }

        // ? DELETE: Remove Destination (only if owned by the user)
        [HttpPost]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            int userId = GetUserId();
            var deletedRows = await _destinationRepository.DeleteDestination(id, userId);

            if (deletedRows == 0)
            {
                return Unauthorized();
            }

            return RedirectToAction("Destinations");
        }

        private int GetUserId()
        {
            return int.TryParse(HttpContext.Session.GetString("UserId"), out int userId) ? userId : 0;
        }
    }
}
