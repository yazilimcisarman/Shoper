using Shoper.Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.AccountServices
{
    public interface IAccountService
    {
        Task<string> Login(LoginDto dto);
        Task<string> Register(RegisterDto dto);
        Task<string> ChangePassword();
        Task Logout();
    }
}
