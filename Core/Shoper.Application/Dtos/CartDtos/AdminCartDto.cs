using Shoper.Application.Dtos.CartItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.CartDtos
{
    public class AdminCartDto
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ResultCartItemDto> CartItems { get; set; }
        public string? UserId { get; set; }
        public string NameSurname { get; set; }
    }
}
