using Shoper.Application.Dtos.CategoryDtos;
using Shoper.Application.Dtos.HelpDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.HelpServices
{
    public interface IHelpService
    {
        Task<List<ResultHelpDto>> GetAllHelpAsync();
        Task<GetByIdHelpDto> GetByIdHelpAsync(int id);
        Task CreateHelpAsync(CreateHelpDto model);
        Task UpdateHelpAsync(UpdateHelpDto model);
        Task DeleteHelpAsync(int id);
        Task<List<ResultHelpDto>> GetByEmailHelpAsync(string email);
    }
}
