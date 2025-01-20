using Microsoft.AspNetCore.Mvc;

namespace CRUD_Dapper.Controllers
{
    public class ChatController : Controller
    {
        public ChatController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
