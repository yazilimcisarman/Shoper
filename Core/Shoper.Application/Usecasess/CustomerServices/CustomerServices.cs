using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.CustomerServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IRepository<Customer> _repository;
        
        public CustomerServices(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            await _repository.CreateAsync(new Customer
            {
                FirstName = createCustomerDto.FirstName,
                LastName = createCustomerDto.LastName,
                Email = createCustomerDto.Email,
                UserId = createCustomerDto.UserId,
                PhoneNumber = createCustomerDto.PhoneNumber,
            });
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(values);
        }

        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new ResultCustomerDto
            {
                CustomerId = x.CustomerId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserId = x.UserId,
                PhoneNumber = x.PhoneNumber,
                //Orders = x.Orders,
            }).ToList();
        }

        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            return new GetByIdCustomerDto
            {
                CustomerId = values.CustomerId,
                FirstName = values.FirstName,
                LastName = values.LastName,
                Email = values.Email,
                UserId = values.UserId,
                PhoneNumber = values.PhoneNumber,
                //Orders = values.Orders,
            };
        }

        public async Task<GetByIdCustomerDto> GetCustomerByUserId(string userid)
        {
            var customer = await _repository.FirstOrDefaultAsync(x => x.UserId == userid);
            return new GetByIdCustomerDto
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                UserId = customer.UserId,
                PhoneNumber = customer.PhoneNumber,
                //Orders = values.Orders,
            };
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var values = await _repository.GetByIdAsync(updateCustomerDto.CustomerId);
            values.FirstName = updateCustomerDto.FirstName;
            values.LastName = updateCustomerDto.LastName;
            values .Email = updateCustomerDto.Email;
            values.PhoneNumber = updateCustomerDto.PhoneNumber;
            //values .Orders = updateCustomerDto.Orders;
            await _repository.UpdateAsync(values);
        }

        public async Task UpdateNameAndSurname(string userId, string name, string surname)
        {
            var values = await _repository.FirstOrDefaultAsync(x => x.UserId == userId);
            values.FirstName = name;
            values.LastName = surname;
            await _repository.UpdateAsync(values);
        }
    }
}
