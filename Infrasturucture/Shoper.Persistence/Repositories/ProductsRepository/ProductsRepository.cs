using Microsoft.EntityFrameworkCore;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Interfaces.IProductsRepository;
using Shoper.Domain.Entities;
using Shoper.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Persistence.Repositories.ProductsRepository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;

        public ProductsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductByCategory(int categoryId)
        {
            return await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        }
    }
}
