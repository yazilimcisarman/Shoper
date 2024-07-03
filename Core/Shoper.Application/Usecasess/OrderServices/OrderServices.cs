using Shoper.Application.Dtos.OrderDtos;
using Shoper.Application.Dtos.OrderItemDtos;
using Shoper.Application.Interfaces;
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

		public OrderServices(IRepository<Order> repository, IRepository<OrderItem> repositoryOrderItem, IRepository<Customer> repositoryCustomer, IRepository<Product> repositoryProduct)
		{
			_repository = repository;
			_repositoryOrderItem = repositoryOrderItem;
			_repositoryCustomer = repositoryCustomer;
			_repositoryProduct = repositoryProduct;
		}

		public async Task CreateOrderAsync(CreateOrderDto model)
        {
            decimal sum = 0;
            var order = new Order
            {
                OrderDate = model.OrderDate,
                TotalAmount = sum,
                OrderStatus = model.OrderStatus,
                //BillingAdress = model.BillingAdress,
                ShippingAdress = model.ShippingAdress,
                //PaymentMethod = model.PaymentMethod,
                CustomerId = model.CustomerId,
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
                    //PaymentMethod = item.PaymentMethod,
                    CustomerId = item.CustomerId,
                    Customer = ordercustomer,
                    OrderItems = new List<ResultOrderItemDto>()
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
                //PaymentMethod = values.PaymentMethod,
                CustomerId = values.CustomerId,
                Customer = ordercustomer,
                OrderItems = new List<ResultOrderItemDto>()
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
