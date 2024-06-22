using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Usecasess.CartItemSevices;

namespace Shoper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _services;

        public CartItemsController(ICartItemService services)
        {
            _services = services;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            var values = await _services.GetAllCartItemAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCartItem(int id)
        {
            var value = await _services.GetByIdCartItemAsync(id);
            return Ok(value);   
        }
        [HttpPost]
        public async Task<IActionResult> CreateCartItem(CreateCartItemDto dto)
        {
            await _services.CreateCartItemAsync(dto);
            return Ok("CartItem başarılı bir şekilde oluşturuldu.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCartItemAsync(UpdateCartItemDto dto)
        {
            await _services.UpdateCartItemAsync(dto);
            return Ok("CartItem başarılı bir şekilde güncellendi.");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            await _services.DeleteCartItemAsync(id);
            return Ok("CartItem başarılı bir şekilde silindi.");
        }
    }
}
