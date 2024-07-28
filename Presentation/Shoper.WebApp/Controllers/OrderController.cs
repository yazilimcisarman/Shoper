using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ICartService _cartService;

        public OrderController(IOrderServices orderServices, ICartService cartService)
        {
            _orderServices = orderServices;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Checkout(int cartId)
        {
            var value = await _cartService.GetByIdCartAsync(cartId);
            if(value == null)
            {
                return View();
            }
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto, int cartId)
        {
            try
            {
                var cart = await _cartService.GetByIdCartAsync(cartId);
                List<CreateOrderItemDto> result = new List<CreateOrderItemDto>();
                foreach (var item in cart.CartItems)
                {
                    var newOrderItem = new CreateOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                    };
                    result.Add(newOrderItem);
                }
                dto.CustomerId =1;
                dto.OrderItems = result;
                dto.OrderStatus = "Siparişiniz Alındı.";
                await _orderServices.CreateOrderAsync(dto);
                return Json(new {success=true});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error","Home",ex.Message);
            }
            
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
