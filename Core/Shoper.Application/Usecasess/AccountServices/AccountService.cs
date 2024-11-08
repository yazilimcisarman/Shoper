using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IUserIdentityRepository _userIdentityRepository;

        public AccountService(IUserIdentityRepository userIdentityRepository)
        {
            _userIdentityRepository = userIdentityRepository;
        }

        public async Task<string> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _userIdentityRepository.ChangePasswordAsync(dto);
            return result;
        }

        public Task<string> GetUserIdAsync(ClaimsPrincipal user)
        {
            var userid = _userIdentityRepository.GetUserIdOnAuth(user);
            return userid;
        }

        public async Task<string> Login(LoginDto dto)
        {
            var result = await _userIdentityRepository.LoginAsync(dto);
            return result;
        }

        public async Task Logout()
        {
            await _userIdentityRepository.LogoutAsync();
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var result = await _userIdentityRepository.RegisterAsync(dto);
            return result;
        }

        public async Task<bool> UpdateUser(string userId, string name, string surname)
        {
            var result = await _userIdentityRepository.UpdateUserNameAndSurnameAsync(userId, name, surname);
            return result;
        }
    }
}
