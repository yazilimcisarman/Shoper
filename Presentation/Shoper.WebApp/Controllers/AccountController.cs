using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.AccountServices;

namespace Shoper.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
