using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.FavoritesDtos
{
    public class AdminFavoritesDto
    {
        public string UserId { get; set; }
        public string NameSurname { get; set; }
        public List<AdminFavoritesDetailDto> FavoritesDetails { get; set; }
    }
}
