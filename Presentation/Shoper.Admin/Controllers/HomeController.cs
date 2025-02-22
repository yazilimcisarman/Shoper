using Microsoft.AspNetCore.Mvc;
using Shoper.Admin.Models;
using Shoper.Application.Usecasess.CategoryServices;
using Shoper.Application.Usecasess.OrderServices;
using System.Diagnostics;

namespace Shoper.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryServices _categoryServices;
        private readonly IOrderServices _orderServices;

        public HomeController(ILogger<HomeController> logger, ICategoryServices categoryServices, IOrderServices orderServices)
        {
            _logger = logger;
            _categoryServices = categoryServices;
            _orderServices = orderServices;
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

        public async Task<IActionResult> GetOrderWithCategory()
        {
            try
            {
                var value = await _orderServices.GetOrderByKategori();
                return Json(new { success = true, data = value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        } 
        public async Task<IActionResult> GetSoledProducts()
        {
            try
            {
                var value = await _orderServices.GetSoledProducts();
                return Json(new { success = true, data = value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public async Task<IActionResult> GetOrderStatus()
        {
            try
            {
                var value = await _orderServices.GetOrderStatusGrafiks();
                return Json(new { success = true, data = value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public async Task<IActionResult> GetSalesTrends()
        {
            try
            {
                var value = await _orderServices.GetSalesTrends();
                return Json(new { success = true, data = value });
            }
            catch (Exception ex )
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public async Task<IActionResult> GetDashboardCards()
        {
            try
            {
                var value = await _orderServices.GetDashboardCards();
                return Json(new { success = true, data = value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
