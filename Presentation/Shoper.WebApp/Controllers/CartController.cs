using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CartDtos;
using Shoper.Application.Dtos.CartItemDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Domain.Entities;
using System.Text.Json;

namespace Shoper.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartservices;
        private readonly ICartItemService _cartitemservice;
        private readonly IProductService _productservice;
        private readonly IUserIdentityRepository _useridentityrepository;

        public CartController(ICartService cartservices, ICartItemService cartitemservice, IProductService productservice, IUserIdentityRepository useridentityrepository)
        {
            _cartservices = cartservices;
            _cartitemservice = cartitemservice;
            _productservice = productservice;
            _useridentityrepository = useridentityrepository;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (Request.Cookies.TryGetValue("cart", out string cartData))
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems = new CreateCartDto();

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData1))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData1) ?? new CreateCartDto();
                    }

                    // `GetByIdCartDto` verisini toplamak için bir liste oluşturun
                    GetByIdCartDto detailedCartItems = new GetByIdCartDto();
                    detailedCartItems.CartItems = new List<ResultCartItemDto>();
                    var totalsum = 0;
                    foreach (var item in cartItems.CartItems)
                    {
                        // Veritabanından veya servisten `GetByIdCartDto` tipinde detaylı bilgi alın

                        var detailedItem = await _productservice.GetByIdProductAsync(item.ProductId);
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
                return View();
            }
            else
            {
                var userId = await _useridentityrepository.GetUserIdOnAuth(User);
                var value = await _cartservices.GetByUserIdCartAsync(userId);
                return View(value);
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddToCartItem([FromBody] CreateCartItemDto model)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    //model.CartId = 2;
                    var userid = await _useridentityrepository.GetUserIdOnAuth(User);
                    var checkcart = await _cartservices.CheckCartAsync(userid);
                    if (checkcart)
                    {
                        var cart = await _cartservices.GetByUserIdCartAsync(userid);
                        var check = await _cartitemservice.CheckCartItems(cart.CartId, model.ProductId);
                        if (check)
                        {
                            await _cartitemservice.UpdateQuantity(cart.CartId, model.ProductId, model.Quantity);
                        }
                        else
                        {
                            model.CartId = cart.CartId;
                            await _cartitemservice.CreateCartItemAsync(model);
                        }
                        var sumprice = cart.TotalAmount + model.TotalPrice;
                        await _cartservices.UpdateTotalAmount(cart.CartId, sumprice);
                    }
                    else
                    {
                        var newcart = new CreateCartDto { CreatedDate = DateTime.Now, UserId = userid, CartItems=new List<CreateCartItemDto>() };
                        await _cartservices.CreateCartAsync(newcart);

                        var cart = await _cartservices.GetByUserIdCartAsync(userid);
                        var check = await _cartitemservice.CheckCartItems(model.CartId, model.ProductId);
                        if (check)
                        {
                            await _cartitemservice.UpdateQuantity(model.CartId, model.ProductId, model.Quantity);
                        }
                        else
                        {
                            model.CartId = cart.CartId;
                            await _cartitemservice.CreateCartItemAsync(model);
                        }
                        var sumprice = cart.TotalAmount + model.TotalPrice;
                        await _cartservices.UpdateTotalAmount(model.CartId, sumprice);
                    }
                    


                    
                }
                else
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems;

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                    }
                    else
                    {
                        cartItems = new CreateCartDto
                        {
                            CartItems = new List<CreateCartItemDto>()  
                        };
                    }

                    var existingItem = cartItems.CartItems.FirstOrDefault(item => item.ProductId == model.ProductId);

                    if (existingItem != null)
                    {
                        // Ürün zaten varsa, Quantity değerini artır
                        existingItem.Quantity += model.Quantity;
                        existingItem.TotalPrice += model.TotalPrice;
                    }
                    else
                    {
                        // Ürün yoksa, yeni ürünü CartItems listesine ekle
                        cartItems.CartItems.Add(model);
                    }


                    var updatedCartData = JsonSerializer.Serialize(cartItems);
                    Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true
                    });
                }
                   
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
            
        }
        [HttpGet]
        public async Task<JsonResult> DeleteCartItem(int id, int productId)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                { 
                    if (id == 0) { return Json(new { error = "Ürün bulunamadı" }); }
                    var cartItem = await _cartitemservice.GetByIdCartItemAsync(id);
                    if(cartItem == null) { return Json(new { error = "Ürün bulunamadı." }); }
                    await _cartitemservice.DeleteCartItemAsync(id);
                    var cart = await _cartservices.GetByIdCartAsync(cartItem.CartId);
                    var tempcartToltal = cart.TotalAmount - cartItem.TotalPrice;
                    await _cartservices.UpdateTotalAmount(cart.CartId, tempcartToltal);
                }
                else
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems;

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                    }
                    else
                    {
                        cartItems = new CreateCartDto
                        {
                            CartItems = new List<CreateCartItemDto>()
                        };
                    }

                    var existingItem = cartItems.CartItems.FirstOrDefault(item => item.ProductId == productId);
                    if (existingItem != null)
                    {
                        cartItems.CartItems.Remove(existingItem);
                    }
                    else
                    {

                        cartItems.CartItems.Add(new CreateCartItemDto());
                    }
                    var updatedCartData = JsonSerializer.Serialize(cartItems);
                    Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true
                    });
                }

                return Json(new { success = true });  
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantityOnCart(UpdateCartItemDto dto)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var cart = await _cartservices.GetByIdCartAsync(dto.CartId);
                    await _cartitemservice.UpdateQuantity(dto.CartId, dto.ProductId, dto.Quantity);
                    var cartitem = await _cartitemservice.GetByIdCartItemAsync(dto.CartItemId);
                    var product = await _productservice.GetByIdProductAsync(dto.ProductId);
                    decimal sumprice = cart.TotalAmount;
                    if (cartitem.Quantity == 0)
                    {
                        await _cartitemservice.DeleteCartItemAsync(cartitem.CartItemId);
                    }
                    if (dto.Quantity > 0)
                    {
                        sumprice = cart.TotalAmount + product.Price;

                    }
                    else
                    {
                        sumprice = cart.TotalAmount - product.Price;
                    }
                    await _cartservices.UpdateTotalAmount(cart.CartId, sumprice);
                }
                else
                {
                    string cookieName = "cart";
                    CreateCartDto cartItems;

                    if (Request.Cookies.TryGetValue(cookieName, out string cartData))
                    {
                        cartItems = JsonSerializer.Deserialize<CreateCartDto>(cartData) ?? new CreateCartDto();
                    }
                    else
                    {
                        cartItems = new CreateCartDto
                        {
                            CartItems = new List<CreateCartItemDto>()
                        };
                    }

                    var existingItem = cartItems.CartItems.FirstOrDefault(item => item.ProductId == dto.ProductId);
                    var cookieproduct = await _productservice.GetByIdProductAsync(dto.ProductId);
                    if (existingItem != null)
                    {
                        if (dto.Quantity > 0)
                        {
                            existingItem.Quantity += dto.Quantity;
                            existingItem.TotalPrice = Convert.ToInt32(existingItem.Quantity*cookieproduct.Price);
                        }
                        else
                        {
                            
                           
                            existingItem.Quantity += dto.Quantity;
                            existingItem.TotalPrice = Convert.ToInt32(existingItem.Quantity * cookieproduct.Price);
                            if (existingItem.TotalPrice == 0)
                            {
                                cartItems.CartItems.Remove(existingItem);
                            }
                           
                        }
                    }
                    else
                    {

                        cartItems.CartItems.Add(new CreateCartItemDto());
                    }


                    var updatedCartData = JsonSerializer.Serialize(cartItems);
                    Response.Cookies.Append(cookieName, updatedCartData, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true
                    });

                }
                   
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex });
            }
           
        }
    }
}
