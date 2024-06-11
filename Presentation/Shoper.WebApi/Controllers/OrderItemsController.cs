using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Usecasess.OrderItemServices;

namespace Shoper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderItem()
        {
            var values = await _orderItemService.GetAllOrderItemAsync();
            return Ok(values);  
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdOrderItem(int id)
        {
            var values = await _orderItemService.GetByIdOrderItemAsync(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(CreateOrderItemDto dto)
        {
            await _orderItemService.CreateOrderItemAsync(dto);
            return Ok("OrderItem başarılı bir şekilde oluşturuldu.");    
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemDto dto)
        {
            await _orderItemService.UpdateOrderItemAsync(dto);
            return Ok("OrderItem başarılı bir şekilde güncellendi.");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return Ok("ORder Item başarılı bir şekilde silindi.");
        }
    }
}
