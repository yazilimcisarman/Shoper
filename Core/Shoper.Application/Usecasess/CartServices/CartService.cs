using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartsRepository;
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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ICartsRepository _cartsRepository;

        public CartService(IRepository<Cart> repository, IRepository<CartItem> itemRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository, ICartsRepository cartsRepository)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _cartsRepository = cartsRepository;
        }

        public async Task CreateCartAsync(CreateCartDto model)
        {
            var cart = new Cart 
            {
                //TotalAmount = model.TotalAmount,
                CreatedDate = DateTime.Now,
                CustomerId = model.CustomerId,
            };
            await _repository.CreateAsync(cart);
            var sum = 0;
            foreach (var item in model.CartItems)
            {
                var cartitem = new CartItem {
					CartId = cart.CartId,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					TotalPrice = item.TotalPrice, // üründen gidip fiyatı alacak ve adet ile çarpacak
				};
                sum = sum + (item.TotalPrice);
                await _itemRepository.CreateAsync(cartitem);
            }
            cart.TotalAmount = sum;
            await _repository.UpdateAsync(cart);


        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);
            var cartItems = await _itemRepository.GetAllAsync();
            foreach (var item in cartItems)
            {
                if (item.CartId == id)
                {
                    var cartitem = await _itemRepository.GetByIdAsync(item.CartItemId);
                    await _itemRepository.DeleteAsync(cartitem);
                }
            }
            await _repository.DeleteAsync(cart);
        }

        public async Task<List<ResultCartDto>> GetAllCartAsync()
        {
            var carts = await _repository.GetAllAsync();
            var cartItems = await _itemRepository.GetAllAsync();
            var product = await _productRepository.GetAllAsync();
            var result = new List<ResultCartDto>();
            foreach (var item in carts)
            {
                var customerdto = await _customerRepository.GetByFilterAsync(cus => cus.CustomerId == item.CustomerId);
				var cartdto = new ResultCartDto
				{
					CartId = item.CartId,
					CreatedDate = item.CreatedDate,
					CustomerId = item.CustomerId,
					Customer = customerdto,
					TotalAmount = item.TotalAmount,
                    CartItems = new List<ResultCartItemDto>()
				};
                foreach (var item1 in item.CartItems)
                {
                    var prodcutdto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
                    var cartItemddto = new ResultCartItemDto
                    {
                        CartId = item1.CartId,
                        CartItemId = item1.CartItemId,
                        ProductId = item1.ProductId,
                        Product = prodcutdto,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                    };
                    cartdto.CartItems.Add(cartItemddto);
                }
                result.Add(cartdto);
			}
                
            return result;
        }

        public async Task<GetByIdCartDto> GetByIdCartAsync(int id)
        {
            var cart = await  _repository.GetByIdAsync(id);
            var cartItem = await _itemRepository.GetAllAsync();
            var customer = await _customerRepository.GetByIdAsync(id);

            var result = new GetByIdCartDto
            {
                CartId = cart.CartId,
                CartItems = new List<ResultCartItemDto>(),
                CreatedDate = cart.CreatedDate,
                CustomerId = cart.CustomerId,
                Customer = customer,
                TotalAmount = cart.TotalAmount,
              
            };
            foreach (var item1 in cart.CartItems)
            {
				var prodcutdto = await _productRepository.GetByFilterAsync(prd => prd.ProductId == item1.ProductId);
				var cartItemddto = new ResultCartItemDto
				{
					CartId = item1.CartId,
					CartItemId = item1.CartItemId,
					ProductId = item1.ProductId,
					Product = prodcutdto,
					Quantity = item1.Quantity,
					TotalPrice = item1.TotalPrice,
				};
				result.CartItems.Add(cartItemddto);
			}
            return result;
        }

        public async Task UpdateCartAsync(UpdateCartDto model)
        {
            var cart = await _repository.GetByIdAsync(model.CartId);
            var cartItems = await _itemRepository.GetAllAsync();
            //cart.CreatedDate = model.CreatedDate;
            //cart.CustomerId = model.CustomerId;
            //cart.TotalAmount = model.TotalAmount;
            var sum = 0;
			foreach (var item1 in model.CartItems)
			{
				foreach (var item in cart.CartItems)
                {

                    var cartItem = await _itemRepository.GetByIdAsync(item.CartItemId);
                
                    if(item.CartItemId == item1.CartItemId)
                    {
                        cartItem.Quantity = item1.Quantity;
                        cartItem.TotalPrice = item1.TotalPrice;
					}
                    sum = sum + item.TotalPrice;
				}
			}
            cart.TotalAmount = sum;
            await _repository.UpdateAsync(cart);
        }

        public async Task UpdateTotalAmount(int cartId, decimal totalAmount)
        {
           await _cartsRepository.UpdateTotalAmountAsync(cartId, totalAmount);
        }
    }
}
