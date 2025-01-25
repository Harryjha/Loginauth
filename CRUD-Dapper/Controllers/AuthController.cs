using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Dapper.Controllers
{
    public class AuthController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        public AuthController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handle Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail("Users", email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Set session
            HttpContext.Session.SetString("UserId", user.id.ToString());
            HttpContext.Session.SetString("Email", user.Email);

            return RedirectToAction("Index", "Home");
        }

        // Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle Registration
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword)
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
                CreatedDate = DateTime.UtcNow,
            };

            await _userRepository.Add("Users", newUser);

            ViewBag.Message = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Helper: Hash Password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Helper: Verify Password
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}

