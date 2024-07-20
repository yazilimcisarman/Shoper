using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartItemsRepository;
using Shoper.Application.Interfaces.ICartsRepository;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.CartItemSevices
{
    public class CartItemService : ICartItemService
    {
        private readonly IRepository<CartItem> _repository;
        private readonly ICartItemsRepository _cartItemsRepository;

        public CartItemService(IRepository<CartItem> repository, ICartItemsRepository cartItemsRepository)
        {
            _repository = repository;
            _cartItemsRepository = cartItemsRepository;
        }

        public async Task<bool> CheckCartItems(int cartId, int productId)
        {
            var value = await _cartItemsRepository.CheckCartItemAsync(cartId, productId);
            return value;
        }

        public async Task CreateCartItemAsync(CreateCartItemDto model)
        {
            var cartItem = new CartItem 
            {
                CartId = model.CartId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                TotalPrice = model.TotalPrice
            };
            await _repository.CreateAsync(cartItem);
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var carItem = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(carItem);
        }

        public async Task<List<ResultCartItemDto>> GetAllCartItemAsync()
        {
            var cartItems = await _repository.GetAllAsync();
            return cartItems.Select(x => new ResultCartItemDto 
            {
                CartId = x.CartId,
                ProductId = x.ProductId,
                CartItemId = x.CartItemId,
                Quantity = x.Quantity,
                TotalPrice = x.TotalPrice,
            }).ToList();
        }

		public Task<List<ResultCartItemDto>> GetByCartIdCartItemAsync(int cartId)
		{
			throw new NotImplementedException();
		}

		public async Task<GetByIdCartItemDto> GetByIdCartItemAsync(int id)
        {
            var cartItem = await _repository.GetByIdAsync(id);
            return new GetByIdCartItemDto 
            {
                Quantity = cartItem.Quantity,
                CartItemId = cartItem.CartItemId,
                ProductId= cartItem.ProductId,
                CartId= cartItem.CartId,
                TotalPrice = cartItem.TotalPrice
            };
        }

        public async Task UpdateCartItemAsync(UpdateCartItemDto model)
        {
            var cartItem = await _repository.GetByIdAsync(model.CartItemId);
            cartItem.Quantity = model.Quantity;
            cartItem.TotalPrice = model.TotalPrice;
            cartItem.ProductId = model.ProductId;
            //cartItem.CartId = model.CartId;
            await _repository.UpdateAsync(cartItem);
        }

        public async Task UpdateQuantity(int cartId, int productId, int quantity)
        {
            await _cartItemsRepository.UpdateQuantity(cartId,productId,quantity);
        }
    }
}
