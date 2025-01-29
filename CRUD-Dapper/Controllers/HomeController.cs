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

        public async Task<IActionResult> Destinations()
        {
            var destinations = await _destinationRepository.GetAllDestinations();
            return View(destinations); // Ensure this matches your view name
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
