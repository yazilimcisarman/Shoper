using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; //Bunu kurmanız gerekecek
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Interfaces;
using Shoper.Persistence.Context.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
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

        public async Task<string> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return "User not found";
            }

            // Yeni şifre ve onay şifresini kontrol et
            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                return "New password and confirmation do not match";
            }

            // Şifreyi değiştir
            var result = await _userManager.ChangePasswordAsync(user, dto.Password, dto.NewPassword);

            if (result.Succeeded)
            {
                return "Password changed successfully";
            }

            // Hata mesajlarını birleştir
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return $"Failed to change password: {errors}";
        }

        public async Task<string> GetUserIdOnAuth(ClaimsPrincipal user)
        {
            string userId = _userManager.GetUserId(user);
            if(userId == null)
            {
                userId = "1111111111111";
            }
            return userId;
        }

        public Task<bool> IsUserAuthenticated()
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

            var result = await _userManager.CreateAsync(user,dto.Password);
            if (result.Succeeded)
            {
                var result1 = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);
                if (result1.Succeeded) {
                    return user.Id;

                }
                return "Üye olundu. Giri; yapilmadi";
            }
            else
            {
                return result.Errors.ToString();
            }
        }
        public async Task<bool> UpdateUserNameAndSurnameAsync(string userId, string newName, string newSurname)
        {
            // Kullanıcıyı ID ile bul
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Yeni ad ve soyadı ayarla
            user.Name = newName; // FirstName ve LastName alanlarının ApplicationUser modelinde tanımlı olduğunu varsayıyorum.
            user.SurName = newSurname;

            // Güncelleme işlemi
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
