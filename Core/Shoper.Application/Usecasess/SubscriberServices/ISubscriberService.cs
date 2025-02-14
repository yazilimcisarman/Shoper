using Shoper.Application.Dtos.SubscriberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.SubscriberServices
{
    public interface ISubscriberService
    {
        Task<List<ResultSubscriberDto>> GetAllSubscribers();
        Task<GetByIdSubscriberDto> GetByIdSubscriber(int id);
        Task CreateSubscriber(CreateSubscriberDto dto);
        Task UpdateSubscriber(UpdateSubscriberDto dto);
        Task DeleteSubscriber(int id);
        Task<List<ResultSubscriberDto>> GetByEmailSubscriber(string email);
    }
}
