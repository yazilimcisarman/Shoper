using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.DashboardDtos
{
    public class DashboardCardsDto
    {
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalCartItemsProducts { get; set; }
        public int CriticStockProducts { get; set; }
    }
}
