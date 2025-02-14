using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Dtos.SubscriberDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.SubscriberServices
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IRepository<Subscriber> _repository;

        public SubscriberService(IRepository<Subscriber> repository)
        {
            _repository = repository;
        }

        public async Task CreateSubscriber(CreateSubscriberDto dto)
        {
            var subscriber = new Subscriber()
            {
                Email = dto.Email,
                Name = dto.Name,
                SubcribeDate = dto.SubcribeDate,
            };
            await _repository.CreateAsync(subscriber);
        }

        public async Task DeleteSubscriber(int id)
        {
            var subscriber = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(subscriber);
        }

        public async Task<List<ResultSubscriberDto>> GetAllSubscribers()
        {
            var subscribers = await _repository.GetAllAsync();
            return subscribers.Select(x => new ResultSubscriberDto
            {
              Id = x.Id,
              Email = x.Email,
              Name = x.Name,
              SubcribeDate = x.SubcribeDate,
            }).ToList();
        }

        public async Task<List<ResultSubscriberDto>> GetByEmailSubscriber(string email)
        {
            Expression<Func<Subscriber, bool>> filter = x => x.Email == email;
            var subscribers = await _repository.WhereAsync(filter);
            return subscribers.Select(x => new ResultSubscriberDto
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                SubcribeDate = x.SubcribeDate,
            }).ToList();
        }

        public async Task<GetByIdSubscriberDto> GetByIdSubscriber(int id)
        {
            var subscriber = await _repository.GetByIdAsync(id);
            var newsubscriber = new GetByIdSubscriberDto
            {
                Id = subscriber.Id,
                Email = subscriber.Email,
                Name = subscriber.Name,
                SubcribeDate = subscriber.SubcribeDate,
            };
            return newsubscriber;
        }

            public async Task UpdateSubscriber(UpdateSubscriberDto dto)
        {
            var subscriber = await _repository.GetByIdAsync(dto.Id);
            subscriber.Name = dto.Name;
            subscriber.Email = dto.Email;
            subscriber.SubcribeDate = dto.SubcribeDate;
            await _repository.UpdateAsync(subscriber);
        }
    }
}
