using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerServices _customerServices;

        public AccountController(IAccountService accountService, ICustomerServices customerServices)
        {
            _accountService = accountService;
            _customerServices = customerServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var values = await _accountService.Login(dto);

            return RedirectToAction("Index","Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var values = await _accountService.Register(dto);

            var customer = new CreateCustomerDto 
            {
                Email = dto.Email,
                FirstName = dto.Name,
                LastName = dto.Surname,
                UserId = values,
            };
            await _customerServices.CreateCustomerAsync(customer);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
