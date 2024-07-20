using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Interfaces.ICartsRepository
{
    public interface ICartsRepository
    {
        Task UpdateTotalAmountAsync(int cartId, decimal totalPrice);
    }
}
