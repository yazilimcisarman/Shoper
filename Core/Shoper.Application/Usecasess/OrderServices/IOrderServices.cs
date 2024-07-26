using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.TownDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.OrderServices
{
    public interface IOrderServices
    {
        Task<List<ResultOrderDto>> GetAllOrderAsync();
        Task<GetByIdOrderDto> GetByIdOrderAsync(int id);
        Task CreateOrderAsync(CreateOrderDto model);
        Task UpdateOrderAsync(UpdateOrderDto model);
        Task DeleteOrderAsync(int id);
        Task<List<ResultCityDto>> GetAllCity();
        Task<List<ResultTownDto>> GetTownByCityId(int cityId);
    }
}
