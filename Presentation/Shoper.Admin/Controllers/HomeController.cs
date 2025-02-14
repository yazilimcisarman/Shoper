using Microsoft.AspNetCore.Mvc;
using Shoper.Admin.Models;
using Shoper.Application.Usecasess.CategoryServices;
using System.Diagnostics;

namespace Shoper.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryServices _categoryServices;

        public HomeController(ILogger<HomeController> logger, ICategoryServices categoryServices)
        {
            _logger = logger;
            _categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryServices.GetAllCategoryAsync();
            return View(categories);
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
