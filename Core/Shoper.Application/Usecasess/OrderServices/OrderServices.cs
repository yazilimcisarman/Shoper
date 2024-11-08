using Shoper.Application.Dtos.CityDtos;
using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Dtos.TownDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.IOrderRepository;
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
        private readonly IOrderRepository _orderRepository;

        public OrderServices(IRepository<Order> repository, IRepository<OrderItem> repositoryOrderItem, IRepository<Customer> repositoryCustomer, IRepository<Product> repositoryProduct, IOrderRepository orderRepository)
        {
            _repository = repository;
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryCustomer = repositoryCustomer;
            _repositoryProduct = repositoryProduct;
            _orderRepository = orderRepository;
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
            foreach (var item in result.OrderItems)
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
    }
}
