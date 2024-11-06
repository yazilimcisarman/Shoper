using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
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

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Content("Kullanýcý giriþ yapmýþ.");
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
