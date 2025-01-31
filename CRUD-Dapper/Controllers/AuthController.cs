using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Dapper.Controllers
{
    public class AuthController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        public AuthController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ Login Page (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Handle Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail("Users", email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Store session data
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role); // Store user role

            return RedirectToAction("Index", "Home");
        }

        // ✅ Register Page (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Handle Registration (POST)
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, string role)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var existingUser = await _userRepository.GetByEmail("Users", email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email is already registered.";
                return View();
            }

            var newUser = new User
            {
                Email = email,
                PasswordHash = HashPassword(password),
                Role = role,  // Owner or Customer
                CreatedDate = DateTime.UtcNow
            };

            await _userRepository.Add("Users", newUser);

            ViewBag.Message = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 🔹 Helper: Hash Password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // 🔹 Helper: Verify Password
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
