using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.AccountDtos;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.OrderItemServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Domain.Entities;

namespace Shoper.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerServices _customerServices;
        private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly IOrderServices _orderServices;
        private readonly IOrderItemService _orderItemService;

        public AccountController(IAccountService accountService, ICustomerServices customerServices, IUserIdentityRepository userIdentityRepository, IOrderServices orderServices, IOrderItemService orderItemService)
        {
            _accountService = accountService;
            _customerServices = customerServices;
            _userIdentityRepository = userIdentityRepository;
            _orderServices = orderServices;
            _orderItemService = orderItemService;
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

            return RedirectToAction("Index", "Home");
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
                PhoneNumber = dto.PhoneNumber,
            };
            await _customerServices.CreateCustomerAsync(customer);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Profile()
        {
            var userid = await _userIdentityRepository.GetUserIdOnAuth(User);
            var user = await _customerServices.GetCustomerByUserId(userid);
            var order = await _orderServices.GetOrderByUserId(userid);
            var result = new ResultProfileDto
            {
                UserId = userid,
                Name = user.FirstName,
                Surname = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Orders = order,
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(string name, string surname)
        {
            var userId = await _accountService.GetUserIdAsync(User);
            var result = await _accountService.UpdateUser(userId, name, surname);
            await _customerServices.UpdateNameAndSurname(userId,name,surname);
            return RedirectToAction("Profile", "Account");

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var userid = await _accountService.GetUserIdAsync(User);
            model.UserId = userid;
            var result = await _accountService.ChangePassword(model);
            return RedirectToAction("Profile","Account");
        }
    }
}
