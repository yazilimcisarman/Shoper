using Microsoft.EntityFrameworkCore;
using Shoper.Application.Interfaces.ICartsRepository;
using Shoper.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Persistence.Repositories.CartsRepository
{
    public class CartsRepository : ICartsRepository
    {
        private readonly AppDbContext _context;

        public CartsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateTotalAmountAsync(int cartId, decimal totalPrice)
        {
            var value = await _context.Carts.Where(x => x.CartId == cartId).FirstOrDefaultAsync();
            if(value != null)
            {
                value.TotalAmount = totalPrice;
            }
            await _context.SaveChangesAsync();
        }
    }
}
