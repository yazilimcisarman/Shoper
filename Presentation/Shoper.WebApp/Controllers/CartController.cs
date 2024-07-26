using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.ProductServices;

namespace Shoper.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartservices;
        private readonly ICartItemService _cartitemservice;
        private readonly IProductService _productservice;

        public CartController(ICartService cartservices, ICartItemService cartitemservice, IProductService productservice)
        {
            _cartservices = cartservices;
            _cartitemservice = cartitemservice;
            _productservice = productservice;
        }

        public async Task<IActionResult> Index(int id=2)
        {
            var value = await _cartservices.GetByIdCartAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<JsonResult> AddToCartItem([FromBody] CreateCartItemDto model)
        {
            try
            {
                model.CartId = 2;
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
        [HttpGet]
        public async Task<JsonResult> DeleteCartItem(int id)
        {
            try
            {
                if(id == 0) { return Json(new { error = "Ürün bulunamadı" }); }
                var cartItem = await _cartitemservice.GetByIdCartItemAsync(id);
                if(cartItem == null) { return Json(new { error = "Ürün bulunamadı." }); }
                await _cartitemservice.DeleteCartItemAsync(id);
                var cart = await _cartservices.GetByIdCartAsync(cartItem.CartId);
                var tempcartToltal = cart.TotalAmount - cartItem.TotalPrice;
                await _cartservices.UpdateTotalAmount(cart.CartId, tempcartToltal);
                return Json(new { success = true });  
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantityOnCart(UpdateCartItemDto dto)
        {
            try
            {
                var cart = await _cartservices.GetByIdCartAsync(dto.CartId);
                await _cartitemservice.UpdateQuantity(dto.CartId, dto.ProductId, dto.Quantity);
                var cartitem = await _cartitemservice.GetByIdCartItemAsync(dto.CartItemId);
                var product = await _productservice.GetByIdProductAsync(dto.ProductId);
                decimal sumprice = cart.TotalAmount;
                if(cartitem.Quantity == 0)
                {
                    await _cartitemservice.DeleteCartItemAsync(cartitem.CartItemId);
                }
                if (dto.Quantity > 0)
                {
                    sumprice = cart.TotalAmount + product.Price;

                }
                else
                {
                    sumprice = cart.TotalAmount - product.Price;
                }
                await _cartservices.UpdateTotalAmount(cart.CartId, sumprice);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
           
        }
    }
}
