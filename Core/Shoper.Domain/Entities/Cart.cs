using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Domain.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CustomerId { get; set; }
        //public Customer? Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public string? UserId { get; set; }
    }
}
