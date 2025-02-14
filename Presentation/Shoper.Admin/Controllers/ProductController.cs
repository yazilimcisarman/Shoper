using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Usecasess.ProductServices;

namespace Shoper.Admin.Controllers
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
            var products = await _productService.GetAllProductAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto model)
        {
            await _productService.CreateProductAsync(model);
            return RedirectToAction("Index");
        }
        //update methods
        public async Task<IActionResult> Edit(int productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(UpdateProductDto model)
        {
            await _productService.UpdateProductAsync(model);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete( int productId)
        {
            await _productService.DeleteProductAsync(productId);
            return RedirectToAction("Index");
        }
    }
}
