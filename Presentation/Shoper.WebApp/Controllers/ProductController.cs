using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.ProductServices;

namespace Shoper.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetProductByCategory(2);
            return View(values);
        }
    }
}
