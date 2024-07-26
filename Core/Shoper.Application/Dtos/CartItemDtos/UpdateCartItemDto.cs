using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.CartItemDtos
{
    public class UpdateCartItemDto
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        //public Product Products{get: set:}
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
