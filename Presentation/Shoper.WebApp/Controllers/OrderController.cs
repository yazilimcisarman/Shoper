using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.OrderServices;

namespace Shoper.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public async Task<ActionResult> GetCity()
        {
            var values = await _orderServices.GetAllCity();
            return Json(new {success = true, data = values});
        }
        public async Task<ActionResult> GetTownByCityId(int cityId)
        {
            var values = await _orderServices.GetTownByCityId(cityId);
            return Json(new {success = true,data=values});
        }
    }
}
