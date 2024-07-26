using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.TownDtos;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Interfaces.IOrderRepository
{
    public interface IOrderRepository
    {
        Task<List<City>> GetCity();
        Task<List<Town>> GetTownByCityId(int cityid);
    }
}
