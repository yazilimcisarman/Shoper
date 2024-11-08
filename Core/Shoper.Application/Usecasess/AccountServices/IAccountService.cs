using Shoper.Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.AccountServices
{
    public interface IAccountService
    {
        Task<string> Login(LoginDto dto);
        Task<string> Register(RegisterDto dto);
        Task<string> ChangePassword(ChangePasswordDto dto);
        Task Logout();
        Task<bool> UpdateUser(string userId, string name, string surname);
        Task<string> GetUserIdAsync(ClaimsPrincipal user);
    }
}
