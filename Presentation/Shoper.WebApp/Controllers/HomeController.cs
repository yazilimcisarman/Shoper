using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.WebApp.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Shoper.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IUserIdentityRepository _userIdentityRepository;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService, IUserIdentityRepository userIdentityRepository)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
            _userIdentityRepository = userIdentityRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Request.Cookies.TryGetValue("cart", out string cartData))
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems = new CreateCartDto();

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                    }
                    cartItems.CreatedDate = DateTime.Now;
                    var userId = await _userIdentityRepository.GetUserIdOnAuth(User);
                    cartItems.UserId = userId;
                    var result = await _cartService.CheckCartAsync(userId);
                    if (!result)
                    {
                        await _cartService.CreateCartAsync(cartItems);
                    }
                }

                var values1 = await _productService.GetProductTake(8);
                return View(values1);
            }
            else
            {
                string cookieName = "cart";

                if (!Request.Cookies.ContainsKey(cookieName))
                {
                    var cartItem = new CreateCartDto();
                    cartItem.CartItems = new List<CreateCartItemDto>();

                    var cartData = JsonSerializer.Serialize(cartItem);
                    Response.Cookies.Append(cookieName, cartData, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7), 
                        HttpOnly = true,
                        Secure = true 
                    });
                }
            }
            
            var values = await _productService.GetProductTake(8);
            return View(values);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id,
                ExMessage = message,

            };
            return View(error);
        }
    }
}
