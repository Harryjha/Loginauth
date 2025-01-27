using CRUD_Dapper.Models;
using CRUD_Dapper.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Dapper.Controllers
{
    public class BranchController : Controller
    {
        private readonly IGenericRepository<Branch> _iGenericRepository;

        public BranchController(IGenericRepository<Branch> iGenericRepository)
        {
            _iGenericRepository = iGenericRepository;
        }

        // Middleware to check session before action execution
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserId") != null;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _iGenericRepository.GetAll("Branch");
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _iGenericRepository.GetById("Branch", id);
            return View(result);
        }

        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                branch.CreatedDate = DateTime.Now;
                await _iGenericRepository.Add("Branch", branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var _Branch = await _iGenericRepository.GetById("Branch", id);
            return View(_Branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Branch branch)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    branch.id = id;
                    await _iGenericRepository.Update("Branch", branch);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var _Branch = await _iGenericRepository.GetById("Branch", id);
            return View(_Branch);
        }

        [HttpDelete, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            await _iGenericRepository.Delete("Branch", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
