using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartItemSevices;
using System.Text.Json;

namespace Shoper.WebApp.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly ICartItemService _cartItemService;

        public CartSummaryViewComponent(IUserIdentityRepository userIdentityRepository, ICartItemService cartItemService)
        {
            _userIdentityRepository = userIdentityRepository;
            _cartItemService = cartItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int cartItemCount = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _userIdentityRepository.GetUserIdOnAuth(UserClaimsPrincipal);
                var cartItems = await _cartItemService.GetCountCartItemsByCartId(userId);
                if(cartItems == 0)
                {
                    cartItemCount = 0;
                }
                else
                {
                    cartItemCount = cartItems;

                }
            }
            else
            {
                string cookieName = "cart";
                

                if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                {
                    var cart = JsonSerializer.Deserialize<CreateCartDto>(cartData);
                    if (cart != null && cart.CartItems != null)
                    {
                        cartItemCount = cart.CartItems.Sum(item => item.Quantity); // Her ürünün miktarını toplar
                    }
                }
            }
            

            return View(cartItemCount);
        }
    }
}
