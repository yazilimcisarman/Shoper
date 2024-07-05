using Shoper.Application.Dtos.ProductDtos;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Interfaces.IProductsRepository
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetProductByCategory(int categoryId);
    }
}
