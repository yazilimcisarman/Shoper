using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Usecasess.CategoryServices;

namespace Shoper.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _categoryServices.GetAllCategoryAsync();
            return View(category);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto model)
        {
            await _categoryServices.CreateCategoryAsync(model);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int categoryId)
        {
            var category = await _categoryServices.GetByIdCategoryAsync(categoryId);
            return View(category);
        }
        [HttpPost]
        public async  Task<IActionResult> EditCategory(UpdateCategoryDto model)
        {
            await _categoryServices.UpdateCategoryAsync(model);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int categoryId)
        {
            await _categoryServices.DeleteCategoryAsync(categoryId);
            return RedirectToAction("Index");
        }
    }
}
