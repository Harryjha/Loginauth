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
    }
}
