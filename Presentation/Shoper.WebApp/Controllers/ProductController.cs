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

        public async Task<IActionResult> Index(int categoryId, decimal minprice, decimal maxprice, string search)
        {
            if(categoryId != 0) 
            {
                var values = await _productService.GetProductByCategory(categoryId);
                return View(values);
            }
            if(maxprice != 0)
            {
                var values = await _productService.GetProductByPrice(minprice, maxprice);
                return View(values);

            }
            var value = await _productService.GetAllProductAsync();
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> Index (string search)
        {
            if(search == null)
            {
                return View();
            }
            var value = await _productService.GetProductBySearch(search);
            return View(value);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var value = await _productService.GetByIdProductAsync(id);
            return View(value);
        }
    }
}
