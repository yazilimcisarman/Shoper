using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.FavoritesServices;
using System.Text.Json;

namespace Shoper.WebApp.ViewComponents
{
    public class FavoriteSummaryViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;
        private readonly IFavoritesService _favoritesService;
        public FavoriteSummaryViewComponent(IAccountService accountService, IFavoritesService favoritesService)
        {
            _accountService = accountService;
            _favoritesService = favoritesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int favoriteItemCount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userid = await _accountService.GetUserIdAsync(UserClaimsPrincipal);
                var favoritecount = await _favoritesService.GetCountByUserId(userid);
                if (favoritecount == 0)
                {
                    favoriteItemCount = 0;
                }
                else
                {
                    favoriteItemCount = favoritecount;

                }
            }

            return View(favoriteItemCount);
        }
    }
}
