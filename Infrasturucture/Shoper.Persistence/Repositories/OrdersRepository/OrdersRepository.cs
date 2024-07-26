using Microsoft.EntityFrameworkCore;
using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.TownDtos;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Domain.Entities;
using Shoper.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Persistence.Repositories.OrdersRepository
{
    public class OrdersRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetCity()
        {
            var cities = await _context.City.ToListAsync();
            return cities;
        }

        public async Task<List<Town>> GetTownByCityId(int cityid)
        {
            var town = await _context.Town.Where(x => x.CityId == cityid).ToListAsync();
            return town;
        }

    }
}
