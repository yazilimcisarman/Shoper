using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.OrderServices;

namespace Shoper.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        public async Task<IActionResult> Index()
        {
            var value = await _orderServices.GetAllOrderAsync();
            return View(value);
        }
        public async Task<IActionResult> Detail(int orderId)
        {
            var value = await _orderServices.GetByIdOrderAsync(orderId);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string orderStatus)
        {
            await _orderServices.UpdateOrderStatus(orderId,orderStatus);
            return RedirectToAction("Index");
        }
    }
}
