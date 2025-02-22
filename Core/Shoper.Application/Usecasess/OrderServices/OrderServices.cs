using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Dtos.ProductDtos;
using Shoper.Application.Dtos.TownDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Application.Usecasess.DashboardDtos;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.OrderServices
{
    public class OrderServices : IOrderServices
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<OrderItem> _repositoryOrderItem;
        private readonly IRepository<Customer> _repositoryCustomer;
        private readonly IRepository<Product> _repositoryProduct;
        private readonly IRepository<Category> _repositoryCategory;
        private readonly IRepository<CartItem> _repositoryCartItem;
        private readonly IOrderRepository _orderRepository;

        public OrderServices(IRepository<Order> repository, IRepository<OrderItem> repositoryOrderItem, IRepository<Customer> repositoryCustomer, IRepository<Product> repositoryProduct, IOrderRepository orderRepository, IRepository<Category> repositoryCategory, IRepository<CartItem> repositoryCartItem)
        {
            _repository = repository;
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryCustomer = repositoryCustomer;
            _repositoryProduct = repositoryProduct;
            _orderRepository = orderRepository;
            _repositoryCategory = repositoryCategory;
            _repositoryCartItem = repositoryCartItem;
        }

        public async Task CreateOrderAsync(CreateOrderDto model)
        {
            decimal sum = 0;
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = sum,
                OrderStatus = model.OrderStatus,
                //BillingAdress = model.BillingAdress,
                ShippingAdress = model.ShippingAdress,
                ShippingCityId=model.ShippingCityId,
                ShippingTownId = model.ShippingTownId,
                //PaymentMethod = model.PaymentMethod,
                CustomerId = model.CustomerId,
                CustomerName = model.CustomerName,
                CustomerSurname = model.CustomerSurname,
                CustomerEmail = model.CustomerEmail,
                CustomerPhone = model.CustomerPhone,
                UserId = model.UserId,
            };
            await _repository.CreateAsync(order);

            foreach (var item in model.OrderItems)
            {
                await _repositoryOrderItem.CreateAsync(new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                });
                sum = sum + item.TotalPrice;
            }
            order.TotalAmount = sum;
            await _repository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            foreach (var item in values.OrderItems)
            {
                var orderItem = await _repositoryOrderItem.GetByIdAsync(item.OrderItemId);
                await _repositoryOrderItem.DeleteAsync(orderItem);
            }
            await _repository.DeleteAsync(values);
        }

        public async Task<List<ResultCityDto>> GetAllCity()
        {
            var values = await _orderRepository.GetCity();
            return values.Select(x => new ResultCityDto
            {
                Id = x.Id,
                CityId = x.CityId,
                Cityname = x.Cityname,
            }).ToList();
        }

        public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            var values = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<ResultOrderDto>();
            foreach (var item in values)
            {
                 var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new ResultOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<ResultOrderItemDto>(),
                    UserId = item.UserId,
				};
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var orderıtemdto = new ResultOrderItemDto
                    {
						OrderId = item1.OrderId,
						ProductId = item1.ProductId,
						Quantity = item1.Quantity,
						TotalPrice = item1.TotalPrice,
						OrderItemId = item1.OrderItemId,
                        Product = orderıtemproduct,
                       
					};
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            return result;
        }

        public async Task<GetByIdOrderDto> GetByIdOrderAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            var ordercustomer = await _repositoryCustomer.GetByIdAsync(values.CustomerId);
            var orderitemsrepo = await _repositoryOrderItem.WhereAsync(x => x.OrderId == id);
            var result = new GetByIdOrderDto 
            {
                OrderId = values.OrderId,
                OrderDate = values.OrderDate,
                TotalAmount = values.TotalAmount,
                OrderStatus = values.OrderStatus,
                //BillingAdress = values.BillingAdress,
                ShippingAdress = values.ShippingAdress,
                ShippingCityId = values.ShippingCityId,
                ShippingTownId = values.ShippingTownId,
                //PaymentMethod = values.PaymentMethod,
                CustomerId = values.CustomerId,
                CustomerName = values.CustomerName,
                CustomerSurname = values.CustomerSurname,
                CustomerEmail = values.CustomerEmail,
                CustomerPhone = values.CustomerPhone,
                OrderItems = new List<ResultOrderItemDto>(),
                UserId = values.UserId,
            };
            foreach (var item in orderitemsrepo)
            {
                var orderitemproduct = await _repositoryProduct.GetByIdAsync(item.ProductId);
                var orderıtemdto = new ResultOrderItemDto
                {
					OrderId = item.OrderId,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					TotalPrice = item.TotalPrice,
					OrderItemId = item.OrderItemId,
					Product = orderitemproduct,
				};
                result.OrderItems.Add(orderıtemdto);
            }
            return result;
        }

        public async Task<DashboardCardsDto> GetDashboardCards()
        {
            var result = new DashboardCardsDto();

            var totalorder = await _repository.GetAllAsync();
            result.TotalOrders = totalorder.Count();

            var totalcustomer = await _repositoryCustomer.GetAllAsync();
            result.TotalCustomers = totalcustomer.Count();

            var totalcart = await _repositoryCartItem.GetAllAsync();
            result.TotalCartItemsProducts = totalcart.GroupBy(y => y.ProductId).Select(x => x.Key).Count();

            var crisiticstock = await _repositoryProduct.GetAllAsync();
            result.CriticStockProducts = crisiticstock.Where(x => x.Stock < 10).Count();

            return result;
        }

        public async Task<List<SalesWithCategory>> GetOrderByKategori()
        {
            var values = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in values)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderıtemproduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderıtemproduct.ProductId,
                        ProductName = orderıtemproduct.ProductName,
                        Description = orderıtemproduct.Description,
                        Price = orderıtemproduct.Price,
                        Stock = orderıtemproduct.Stock,
                        ImageUrl = orderıtemproduct.ImageUrl,
                        CategoryId = orderıtemproduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderıtemdto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .SelectMany(x => x.OrderItems)
                .GroupBy(y => y.Product.Category.CategoryName)
                .Select(z => new SalesWithCategory
                {
                    Categoryname = z.Key,
                    Totalsales = z.Sum(y => y.TotalPrice)
                })
                .OrderByDescending( a => a.Totalsales).ToList();

            return result1;
        }

        public async Task<List<ResultOrderDto>> GetOrderByUserId(string userId)
        {
            var values = await _repository.WhereAsync(x => x.UserId == userId);
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<ResultOrderDto>();
            foreach (var item in values)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new ResultOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<ResultOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var orderıtemdto = new ResultOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = orderıtemproduct,

                    };
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            return result;
        }

        public async Task<List<DashboardOrderStatusDto>> GetOrderStatusGrafiks()
        {
            var values = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in values)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderıtemproduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderıtemproduct.ProductId,
                        ProductName = orderıtemproduct.ProductName,
                        Description = orderıtemproduct.Description,
                        Price = orderıtemproduct.Price,
                        Stock = orderıtemproduct.Stock,
                        ImageUrl = orderıtemproduct.ImageUrl,
                        CategoryId = orderıtemproduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderıtemdto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .GroupBy(y => y.OrderStatus)
                .Select(z => new DashboardOrderStatusDto
                {
                    Status = z.Key,
                    Count = z.Key.Count()
                }).ToList();

            return result1;
        }

        public async Task<List<SalesTrendDto>> GetSalesTrends()
        {
            var values = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in values)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderıtemproduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderıtemproduct.ProductId,
                        ProductName = orderıtemproduct.ProductName,
                        Description = orderıtemproduct.Description,
                        Price = orderıtemproduct.Price,
                        Stock = orderıtemproduct.Stock,
                        ImageUrl = orderıtemproduct.ImageUrl,
                        CategoryId = orderıtemproduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderıtemdto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .GroupBy(y => y.OrderDate.ToString("yyyy-MM"))
                .Select(z => new SalesTrendDto
                {
                    Months = z.Key.Replace(".",""), 
                    SalesCount = z.Sum(y => y.OrderItems.Sum(x => x.Quantity)).ToString(),
                    TotalAmount = z.Sum(y => y.TotalAmount).ToString().Replace(",","."),
                })
                .OrderBy(a => a.Months).ToList();

            return result1;
        }

        public async Task<List<DashboardSoledProductDto>> GetSoledProducts()
        {
            var values = await _repository.GetAllAsync();
            var orderitem = await _repositoryOrderItem.GetAllAsync();
            var result = new List<DashboardOrderDto>();
            foreach (var item in values)
            {
                var ordercustomer = await _repositoryCustomer.GetByIdAsync(item.CustomerId);
                var orderdto = new DashboardOrderDto
                {
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    OrderStatus = item.OrderStatus,
                    //BillingAdress = item.BillingAdress,
                    ShippingAdress = item.ShippingAdress,
                    ShippingCityId = item.ShippingCityId,
                    ShippingTownId = item.ShippingTownId,
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    CustomerSurname = item.CustomerSurname,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    OrderItems = new List<DashboardOrderItemDto>(),
                    UserId = item.UserId,
                };
                foreach (var item1 in item.OrderItems)
                {
                    var orderıtemproduct = await _repositoryProduct.GetByIdAsync(item1.ProductId);
                    var category = await _repositoryCategory.GetByIdAsync(orderıtemproduct.CategoryId);
                    var newcategory = new GetByIdCategoryDto
                    {
                        CategoryId = category.CategoryId,
                        CategoryName = category.CategoryName,
                    };
                    var dashboardproduct = new DashboardProductDto
                    {
                        ProductId = orderıtemproduct.ProductId,
                        ProductName = orderıtemproduct.ProductName,
                        Description = orderıtemproduct.Description,
                        Price = orderıtemproduct.Price,
                        Stock = orderıtemproduct.Stock,
                        ImageUrl = orderıtemproduct.ImageUrl,
                        CategoryId = orderıtemproduct.CategoryId,
                        Category = newcategory,
                    };
                    var orderıtemdto = new DashboardOrderItemDto
                    {
                        OrderId = item1.OrderId,
                        ProductId = item1.ProductId,
                        Quantity = item1.Quantity,
                        TotalPrice = item1.TotalPrice,
                        OrderItemId = item1.OrderItemId,
                        Product = dashboardproduct,

                    };
                    orderdto.OrderItems.Add(orderıtemdto);
                }
                result.Add(orderdto);
            }
            var result1 = result
                .SelectMany(x => x.OrderItems)
                .GroupBy(y => y.Product.ProductName)
                .Select(z => new DashboardSoledProductDto
                {
                    ProductName = z.Key,
                    TotalSoled = z.Sum(y => y.Quantity)
                })
                .OrderByDescending(a => a.TotalSoled).Take(5).ToList();

            return result1;
        }

        public async Task<List<ResultTownDto>> GetTownByCityId(int cityId)
        {
            var values = await _orderRepository.GetTownByCityId(cityId);
            return values.Select(x => new ResultTownDto
            {
                Id = x.Id,
                CityId = x.CityId,
                TownId = x.TownId,
                Townname = x.Townname,
            }).ToList();
        }

        public async Task UpdateOrderAsync(UpdateOrderDto model)
        {
            var values = await _repository.GetByIdAsync(model.OrderId);
            var orderitems = await _repositoryOrderItem.GetAllAsync();
			values.OrderStatus = model.OrderStatus;
            decimal sum = 0;
            foreach (var item in model.OrderItems)
            {

                foreach (var item1 in values.OrderItems)
                {
                    var orderItemdto = await _repositoryOrderItem.GetByIdAsync(item1.OrderItemId);
                    if(item.OrderItemId == item1.OrderItemId)
                    {
                        orderItemdto.Quantity = item.Quantity;
                        orderItemdto.TotalPrice = item.TotalPrice;
                    }
                    sum = sum + item1.TotalPrice;
                }

            }
            values.TotalAmount = sum;

            await _repository.UpdateAsync(values);
        }

        public async Task UpdateOrderStatus(int orderId, string orderstatus)
        {
            var value = await _repository.GetByIdAsync(orderId);
            value.OrderStatus = orderstatus;
            await _repository.UpdateAsync(value);
        }
    }
}
