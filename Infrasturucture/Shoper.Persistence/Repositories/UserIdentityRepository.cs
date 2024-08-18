using Microsoft.AspNetCore.Identity; //Bunu kurmanız gerekecek
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Interfaces;
using Shoper.Persistence.Context.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Persistence.Repositories
{
    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public UserIdentityRepository(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<string> ChangePasswordAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
            {
                return "Kullanıcıc bulunamadı.";
            }
            var result = await _signInManager.PasswordSignInAsync(dto.Email,dto.Password,true,false);
            if (result.Succeeded)
            {
                return "Giriş Başarılı";
            }
            if (result.IsLockedOut)
            {
                return "Kullanıcı hesabı kilitli";
            }
            if (result.IsNotAllowed)
            {
                return "Giriş izni yok.";
            }
            if (result.RequiresTwoFactor)
            {
                return "İki faktörlü doğrulama gerekli.";
            }
            return "Geçersiz giriş denemesi.";
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
           if(dto.Password != dto.RePassword)
            {
                return "Şifreler uyumlu değil.";
            }
            var user = new AppIdentityUser
            {
                Name = dto.Name,
                SurName = dto.Surname,
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return "Üye olundu.";
            }
            else
            {
                return result.Errors.ToString();
            }
        }
    }
}
