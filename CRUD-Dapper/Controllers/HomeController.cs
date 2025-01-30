using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // GET: /Home/Destinations
        public async Task<IActionResult> Destinations()
        {
            var destinations = await _destinationRepository.GetAllDestinations();
            return View(destinations); // Ensure a "Destinations.cshtml" view exists
        }

        // ? GET: Show Create Destination Form
        public IActionResult CreateDestination()
        {
            return View(); // Ensure "CreateDestination.cshtml" exists
        }

        // ? POST: Submit Form Data
        [HttpPost]
        public async Task<IActionResult> CreateDestination(Destination model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _destinationRepository.AddDestination(model);
            return RedirectToAction("Destinations"); // Redirect to the list after adding
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
