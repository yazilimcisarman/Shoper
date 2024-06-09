using Shoper.Application.Dtos.OrderDtos;
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

        public OrderServices(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task CreateOrderAsync(CreateOrderDto model)
        {
            await _repository.CreateAsync(new Order
            {
                OrderDate = model.OrderDate,
                TotalAmount = model.TotalAmount,
                OrderStatus = model.OrderStatus,
                BillingAdress = model.BillingAdress,
                ShippingAdress = model.ShippingAdress,
                PaymentMethod = model.PaymentMethod,
                CustomerId = model.CustomerId,  
            });
        }

        public async Task DeleteOrderAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(values);
        }

        public async Task<List<ResultOrderDto>> GetAllOrderAsync()
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new ResultOrderDto
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,
                OrderStatus = x.OrderStatus,
                BillingAdress = x.BillingAdress,
                ShippingAdress = x.ShippingAdress,
                PaymentMethod = x.PaymentMethod,
                CustomerId = x.CustomerId,
            }).ToList();
        }

        public async Task<GetByIdOrderDto> GetByIdOrderAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            var result = new GetByIdOrderDto 
            {
                OrderId = values.OrderId,
                OrderDate = values.OrderDate,
                TotalAmount = values.TotalAmount,
                OrderStatus = values.OrderStatus,
                BillingAdress = values.BillingAdress,
                ShippingAdress = values.ShippingAdress,
                PaymentMethod = values.PaymentMethod,
                CustomerId = values.CustomerId,
            };
            return result;
        }

        public async Task UpdateOrderAsync(UpdateOrderDto model)
        {
            var values = await _repository.GetByIdAsync(model.OrderId);
            values.OrderDate = model.OrderDate;
            values.TotalAmount = model.TotalAmount;
            values.OrderStatus = model.OrderStatus;
            values.ShippingAdress = model.ShippingAdress;
            values.PaymentMethod = model.PaymentMethod;
            values.CustomerId = model.CustomerId;
            values.BillingAdress = model.BillingAdress;
            await _repository.UpdateAsync(values);
        }
    }
}
