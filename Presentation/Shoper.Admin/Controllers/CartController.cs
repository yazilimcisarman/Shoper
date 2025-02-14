using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.CartServices;

namespace Shoper.Admin.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var value = await _cartService.GetAllAdminCartAsync();
            return View(value);
        }
    }
}
