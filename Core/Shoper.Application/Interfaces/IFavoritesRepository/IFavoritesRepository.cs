using Shoper.Application.Dtos.FavoritesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Interfaces.IFavoritesRepository
{
    public interface IFavoritesRepository
    {
        Task<List<AdminFavoritesDto>> GetFavoritesGroupUserId();
    }
}
