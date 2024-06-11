using Shoper.Application.Dtos.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.OrderItemServices
{
    public interface IOrderItemService
    {
        Task<List<ResultOrderItemDto>> GetAllOrderItemAsync();
        Task<GetByIdOrderItemDto> GetByIdOrderItemAsync(int id);
        Task CreateOrderItemAsync(CreateOrderItemDto model);
        Task UpdateOrderItemAsync(UpdateOrderItemDto model);
        Task DeleteOrderItemAsync(int id);
    }
}
