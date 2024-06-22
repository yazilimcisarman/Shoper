using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.CartServices
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _repository;
        private readonly IRepository<CartItem> _itemRepository;

        public CartService(IRepository<Cart> repository, IRepository<CartItem> itemRepository)
        {
            _repository = repository;
            _itemRepository = itemRepository;
        }

        public async Task CreateCartAsync(CreateCartDto model)
        {
            var cart = new Cart 
            {
                TotalAmount = model.TotalAmount,
                CreatedDate = DateTime.Now,
                CustomerId = model.CustomerId,
            };
            await _repository.CreateAsync(cart);

            foreach (var item in model.CartItems)
            {
                await _itemRepository.CreateAsync(new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                });
            }

        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(cart);
        }

        public async Task<List<ResultCartDto>> GetAllCartAsync()
        {
            var carts = await _repository.GetAllAsync();
            var cartItems = await _itemRepository.GetAllAsync();
            return carts.Select(x => new ResultCartDto
            {
                CartId=x.CartId,
                CreatedDate=x.CreatedDate,
                CustomerId=x.CustomerId,
                TotalAmount =x.TotalAmount,
                CartItems = x.CartItems.Select(y => new ResultCartItemDto 
                {
                    CartId=y.CartId,
                    CartItemId=y.CartItemId,
                    ProductId=y.ProductId,
                    Quantity=y.Quantity,
                    TotalPrice=y.TotalPrice,
                }).ToList()
            }).ToList();
        }

        public async Task<GetByIdCartDto> GetByIdCartAsync(int id)
        {
            var cart = await  _repository.GetByIdAsync(id);
            var result = new GetByIdCartDto 
            {
                CartId = cart.CartId,
                //CartItems= cart.CartItems,
                CreatedDate = cart.CreatedDate,
                CustomerId = cart.CustomerId
              
            };
            return result;
        }

        public async Task UpdateCartAsync(UpdateCartDto model)
        {
            var cart = await _repository.GetByIdAsync(model.CartId);
            cart.CreatedDate = model.CreatedDate;
            cart.CustomerId = model.CustomerId;
            cart.TotalAmount = model.TotalAmount;
            await _repository.UpdateAsync(cart);
        }
    }
}
