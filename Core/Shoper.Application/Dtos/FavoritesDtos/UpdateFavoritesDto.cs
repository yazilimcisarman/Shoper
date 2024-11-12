using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.FavoritesDtos
{
    public class UpdateFavoritesDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public int? CustomerId { get; set; }
    }
}
