using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Dtos.FavoritesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.FavoritesServices
{
    public interface IFavoritesService
    {
        Task<List<ResultFavoritesDto>> GetAllFavoritesAsync();
        Task<GetByIdFavoritesDto> GetByIdFavoritesAsync(int id);
        Task CreateFavoritesAsync(CreateFavoritesDto createFavoritesDto);
        Task UpdateFavoritesAsync(UpdateFavoritesDto updateFavoritesDto);
        Task DeleteFavoritesAsync(int id);
        Task<List<ResultFavoritesDto>> GetFavoritesByUserId(string userid);
        Task<bool> CheckFavoritesByUseridAndProductId(string userid, int productid);
        Task<int> GetCountByUserId(string userid);
        Task<List<AdminFavoritesDto>> GetAdminFavoritesList();
    }
}
