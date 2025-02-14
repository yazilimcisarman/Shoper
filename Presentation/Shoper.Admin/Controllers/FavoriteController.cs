using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.FavoritesServices;

namespace Shoper.Admin.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoritesService _favoritesService;

        public FavoriteController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        public async Task<IActionResult> Index()
        {
            var value = await _favoritesService.GetAdminFavoritesList();
            return View(value);
        }
    }
}
