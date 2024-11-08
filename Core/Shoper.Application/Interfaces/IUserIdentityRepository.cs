using Shoper.Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Interfaces
{
    public interface IUserIdentityRepository
    {
        Task<string> LoginAsync(LoginDto dto);
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> ChangePasswordAsync(ChangePasswordDto dto);
        Task LogoutAsync();
        Task<bool> IsUserAuthenticated();
        Task<string> GetUserIdOnAuth(ClaimsPrincipal user);
        Task<bool> UpdateUserNameAndSurnameAsync(string userId, string newName, string newSurname);
    }
}
