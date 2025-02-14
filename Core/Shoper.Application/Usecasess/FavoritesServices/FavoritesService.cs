using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Dtos.FavoritesDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IFavoritesRepository;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.FavoritesServices
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IRepository<Favorites> _repository;
        private readonly IRepository<Product> _productRepository;
        private readonly IFavoritesRepository _favoritesRepository;

        public FavoritesService(IRepository<Favorites> repository, IRepository<Product> productRepository, IFavoritesRepository favoritesRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _favoritesRepository = favoritesRepository;
        }

        public async Task<bool> CheckFavoritesByUseridAndProductId(string userid, int productid)
        {
            var favorite = await _repository.WhereAsync(x => x.UserId == userid & x.ProductId == productid);
            if(favorite.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task CreateFavoritesAsync(CreateFavoritesDto createFavoritesDto)
        {
            await _repository.CreateAsync(new Favorites
            {
                CreatedDate = createFavoritesDto.CreatedDate,
                CustomerId = createFavoritesDto.CustomerId,
                ProductId = createFavoritesDto.ProductId,
                UserId = createFavoritesDto.UserId
            });
        }

        public async Task DeleteFavoritesAsync(int id)
        {
            var favorite = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(favorite);
        }

        public Task<List<AdminFavoritesDto>> GetAdminFavoritesList()
        {
            var value = _favoritesRepository.GetFavoritesGroupUserId();
            return value;
        }

        public async Task<List<ResultFavoritesDto>> GetAllFavoritesAsync()
        {
            var values = await _repository.GetAllAsync();
            var result = new List<ResultFavoritesDto>();
            foreach (var value in values)
            {
                var favorites = new ResultFavoritesDto
                {
                    CreatedDate = value.CreatedDate,
                    UserId = value.UserId,
                    CustomerId = value.CustomerId,
                    ProductId = value.ProductId,
                    Id = value.Id,
                    Product = new Product(),
                };
                var product = await _productRepository.GetByIdAsync(value.ProductId);
                favorites.Product = product;
                result.Add(favorites);
            };
            return result;
        }
        public async Task<GetByIdFavoritesDto> GetByIdFavoritesAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            var product = await _productRepository.GetByIdAsync(value.ProductId);
            var newfavorite = new GetByIdFavoritesDto
            {
                CreatedDate = value.CreatedDate,
                ProductId = value.ProductId,
                CustomerId = value.CustomerId,
                UserId = value.UserId,
                Id = value.Id,
                Product = product,
            };
            return newfavorite;
        }

        public async Task<int> GetCountByUserId(string userid)
        {
            var favorite = await _repository.WhereAsync(x => x.UserId == userid);
            return favorite.Count;
        }

        public async Task<List<ResultFavoritesDto>> GetFavoritesByUserId(string userid)
        {
            var values = await _repository.WhereAsync(x => x.UserId == userid);
            var result = new List<ResultFavoritesDto>();
            foreach (var value in values)
            {
                var favorites = new ResultFavoritesDto
                {
                    CreatedDate = value.CreatedDate,
                    UserId = value.UserId,
                    CustomerId = value.CustomerId,
                    ProductId = value.ProductId,
                    Id = value.Id,
                    Product = new Product(),
                };
                var product = await _productRepository.GetByIdAsync(value.ProductId);
                favorites.Product = product;
                result.Add(favorites);
            };
            return result;
        }

        public async Task UpdateFavoritesAsync(UpdateFavoritesDto updateFavoritesDto)
        {
            var value = await _repository.GetByIdAsync(updateFavoritesDto.Id);
            value.ProductId = updateFavoritesDto.ProductId;
            await _repository.UpdateAsync(value);
        }
    }
}
