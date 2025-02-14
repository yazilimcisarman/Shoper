using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.CartServices
{
    public interface ICartService
    {
        Task<List<ResultCartDto>> GetAllCartAsync();
        Task<GetByIdCartDto> GetByIdCartAsync(int id);
        Task CreateCartAsync(CreateCartDto model);
        Task UpdateCartAsync(UpdateCartDto model);
        Task DeleteCartAsync(int id);
        Task UpdateTotalAmount(int cartId, decimal totalAmount);
        Task<GetByIdCartDto> GetByUserIdCartAsync(string userId);
        Task<bool> CheckCartAsync(string userId);
        Task<List<AdminCartDto>> GetAllAdminCartAsync();

    }
}
