using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;

namespace Shoper.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartservices;
        private readonly ICartItemService _cartitemservice;

        public CartController(ICartService cartservices, ICartItemService cartitemservice)
        {
            _cartservices = cartservices;
            _cartitemservice = cartitemservice;
        }

        public async Task<IActionResult> Index(int id=1)
        {
            var value = await _cartservices.GetByIdCartAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<JsonResult> AddToCartItem([FromBody] CreateCartItemDto model)
        {
            try
            {
                model.CartId = 1;
                var cart = await _cartservices.GetByIdCartAsync(model.CartId);
                var check = await _cartitemservice.CheckCartItems(model.CartId,model.ProductId);
                if (check)
                {
                    await _cartitemservice.UpdateQuantity(model.CartId,model.ProductId,model.Quantity);
                }
                else
                {
                    await _cartitemservice.CreateCartItemAsync(model);
                }


                var sumprice = cart.TotalAmount + model.TotalPrice;
                await _cartservices.UpdateTotalAmount(model.CartId,sumprice);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
            
        }
    }
}
