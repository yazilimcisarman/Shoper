using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IRepository<Category> _repository;

        public CategoryServices(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto model)
        {
            await _repository.CreateAsync(new Category
            {
                CategoryName = model.CategoryName,
            });
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(category);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(x => new ResultCategoryDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            }).ToList();

        }

        public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            var newcategory = new GetByIdCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
            return newcategory;

        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto model)
        {
            var category = await _repository.GetByIdAsync(model.CategoryId);
            category.CategoryName = model.CategoryName;
            await _repository.UpdateAsync(category);
        }
    }
}
