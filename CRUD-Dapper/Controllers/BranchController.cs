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

        public IGenericRepository<Branch> IGenericRepositroy { get; }

        public async Task<IActionResult> Index()
        {
            var result = await _iGenericRepository.GetAll("Branch");
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _iGenericRepository.GetById("Branch", id);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch)
        {
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
            var _Branch = await _iGenericRepository.GetById("Branch", id);
            return View(_Branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Branch branch)
        {
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
            var _Branch = await _iGenericRepository.GetById("Branch", id);
            return View(_Branch);
        }

        [HttpDelete, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _iGenericRepository.Delete("Branch", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
