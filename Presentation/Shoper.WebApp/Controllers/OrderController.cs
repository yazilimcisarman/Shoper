using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Dtos.ContactDtos;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Domain.Entities;
using System.Text.Json;

namespace Shoper.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICartItemService _cartItemService;
        private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly ICustomerServices _customerServices;

        public OrderController(IOrderServices orderServices, ICartService cartService, IProductService productService, ICartItemService cartItemService, IUserIdentityRepository userIdentityRepository, ICustomerServices customerServices)
        {
            _orderServices = orderServices;
            _cartService = cartService;
            _productService = productService;
            _cartItemService = cartItemService;
            _userIdentityRepository = userIdentityRepository;
            _customerServices = customerServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Checkout(int cartId)
        {
            if(!User.Identity.IsAuthenticated)
            {
                string cookieName = "cart";
                CreateCartDto cartItems = new CreateCartDto();

                if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                {
                    cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                }
                GetByIdCartDto detailedCartItems = new GetByIdCartDto();
                detailedCartItems.CartItems = new List<ResultCartItemDto>();
                var totalsum = 0;
                foreach (var item in cartItems.CartItems)
                {
                    // Veritabanından veya servisten `GetByIdCartDto` tipinde detaylı bilgi alın

                    var detailedItem = await _productService.GetByIdProductAsync(item.ProductId);
                    var newproduct = new Product();
                    newproduct.ProductId = detailedItem.ProductId;
                    newproduct.ProductName = detailedItem.ProductName;
                    newproduct.Price = detailedItem.Price;
                    newproduct.ImageUrl = detailedItem.ImageUrl;
                    newproduct.Description = detailedItem.Description;
                    newproduct.CategoryId = detailedItem.CategoryId;
                    newproduct.Stock = detailedItem.Stock;

                    totalsum += item.TotalPrice;

                    var newccartitem = new ResultCartItemDto()
                    {
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        TotalPrice = item.TotalPrice,
                        Product = newproduct,
                    };

                    detailedCartItems.CartItems.Add(newccartitem);

                }
                detailedCartItems.TotalAmount = totalsum;
                return View(detailedCartItems);
            }
            else
            {
                var value = await _cartService.GetByIdCartAsync(cartId);
                var userid = await _userIdentityRepository.GetUserIdOnAuth(User);
                var customer = await _customerServices.GetCustomerByUserId(userid);

                value.Customer = new Customer { 
                    CustomerId = customer.CustomerId,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    UserId = customer.UserId
                };
                
                if (value == null)
                {
                    return View();
                }
                return View(value);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto, int cartId)
        {
            try
            {
                var cart = new GetByIdCartDto();

                if (!User.Identity.IsAuthenticated)
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems = new CreateCartDto();

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                    }
                    GetByIdCartDto detailedCartItems = new GetByIdCartDto();
                    detailedCartItems.CartItems = new List<ResultCartItemDto>();
                    var totalsum = 0;
                    foreach (var item in cartItems.CartItems)
                    {
                        // Veritabanından veya servisten `GetByIdCartDto` tipinde detaylı bilgi alın

                        var detailedItem = await _productService.GetByIdProductAsync(item.ProductId);
                        var newproduct = new Product();
                        newproduct.ProductId = detailedItem.ProductId;
                        newproduct.ProductName = detailedItem.ProductName;
                        newproduct.Price = detailedItem.Price;
                        newproduct.ImageUrl = detailedItem.ImageUrl;
                        newproduct.Description = detailedItem.Description;
                        newproduct.CategoryId = detailedItem.CategoryId;
                        newproduct.Stock = detailedItem.Stock;

                        totalsum += item.TotalPrice;

                        var newccartitem = new ResultCartItemDto()
                        {
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            TotalPrice = item.TotalPrice,
                            Product = newproduct,
                        };

                        detailedCartItems.CartItems.Add(newccartitem);

                    }
                    detailedCartItems.TotalAmount = totalsum;
                    detailedCartItems.UserId = "1111111111111";
                    cart = detailedCartItems;
                }
                else
                {
                    cart = await _cartService.GetByIdCartAsync(cartId);
                }
                List<CreateOrderItemDto> result = new List<CreateOrderItemDto>();
                foreach (var item in cart.CartItems)
                {
                    var newOrderItem = new CreateOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                    };
                    result.Add(newOrderItem);
                }
                dto.UserId = await _userIdentityRepository.GetUserIdOnAuth(User);
                dto.CustomerId =1009;
                dto.OrderItems = result;
                dto.OrderStatus = "Siparişiniz Alındı.";
                await _orderServices.CreateOrderAsync(dto);
                if(cart.CartItems != null)
                {
                    foreach (var item in cart.CartItems)
                    {
                        if(item.CartItemId != 0)
                        {
                            await _cartItemService.DeleteCartItemAsync(item.CartItemId);
                        }
                    }
                    if(cartId != 0)
                    {
                        await _cartService.DeleteCartAsync(cartId);
                    }
                }
                string cookieName1 = "cart";

                Response.Cookies.Delete(cookieName1);

                var emptyCart = new CreateCartDto
                {
                    CartItems = new List<CreateCartItemDto>() 
                };

                var cartData2 = JsonSerializer.Serialize(emptyCart);
                Response.Cookies.Append(cookieName1, cartData2, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true
                });

                return Json(new {success=true});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error","Home",ex.Message);
            }
            
        }
        public async Task<ActionResult> GetCity()
        {
            var values = await _orderServices.GetAllCity();
            return Json(new {success = true, data = values});
        }
        public async Task<ActionResult> GetTownByCityId(int cityId)
        {
            var values = await _orderServices.GetTownByCityId(cityId);
            return Json(new {success = true,data=values});
        }
    }
}
