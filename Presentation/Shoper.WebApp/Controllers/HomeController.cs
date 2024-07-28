using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.WebApp.Models;
using System.Diagnostics;

namespace Shoper.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetProductTake(8);
            return View(values);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id,
                ExMessage = message,

            };
            return View(error);
        }
    }
}
