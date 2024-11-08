using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.CartDtos
{
    public class UpdateCartDto
    {
        public int CartId { get; set; }
        //public decimal TotalAmount { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int CustomerId { get; set; }
        //public Customer? Customer { get; set; }
        public ICollection<UpdateCartItemDto> CartItems { get; set; }

    }
}
