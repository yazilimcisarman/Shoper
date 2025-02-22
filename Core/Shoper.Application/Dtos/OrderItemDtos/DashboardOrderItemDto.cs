using Shoper.Application.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.OrderItemDtos
{
    public class DashboardOrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        //public Order Order { get; set; }
        public int ProductId { get; set; }
        public DashboardProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
