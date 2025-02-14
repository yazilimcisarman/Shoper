using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.CustomerDtos;
using Shoper.Application.Interfaces;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.ContactServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.FavoritesServices;
using Shoper.Application.Usecasess.HelpServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Application.Usecasess.SubscriberServices;

namespace Shoper.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerServices _customerServices;
        private readonly IUserIdentityRepository _userIdentityRepository;
        private readonly ICartService _cartservice;
        private readonly IContactService _contactService;
        private readonly IFavoritesService _favoritesService;
        private readonly IHelpService _helpService;
        private readonly IOrderServices _orderServices;
        private readonly ISubscriberService _subscriberService;

        public CustomerController(ICustomerServices customerServices, IUserIdentityRepository userIdentityRepository, ICartService cartservice, IContactService contactService, IFavoritesService favoritesService, IHelpService helpService, IOrderServices orderServices, ISubscriberService subscriberService)
        {
            _customerServices = customerServices;
            _userIdentityRepository = userIdentityRepository;
            _cartservice = cartservice;
            _contactService = contactService;
            _favoritesService = favoritesService;
            _helpService = helpService;
            _orderServices = orderServices;
            _subscriberService = subscriberService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerServices.GetAllCustomerAsync();
            return View(customers);
        }
        public async Task<IActionResult> Edit(int customerId)
        {
            var customer = await _customerServices.GetByIdCustomerAsync(customerId);
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> EditCustomer(UpdateCustomerDto model)
        {
            var customeruserID = await _customerServices.GetByIdCustomerAsync(model.CustomerId);
            await _customerServices.UpdateNameAndSurname(customeruserID.UserId,model.FirstName,model.LastName);
            var result = await _userIdentityRepository.UpdateUserNameAndSurnameAsync(customeruserID.UserId,model.FirstName,model.LastName);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Detail(int customerId)
        {
            var customer = new CustomerDetailDto();
            customer.CustomerId = customerId;
            var dbcustomer = await _customerServices.GetByIdCustomerAsync(customerId);
            customer.Customer = dbcustomer;
            var dbcart = await _cartservice.GetByUserIdCartAsync(dbcustomer.UserId);
            customer.Cart = dbcart;
            var dbcontacts = await _contactService.GetAllContactsByEmailAsync(dbcustomer.Email);
            customer.Contacts = dbcontacts;
            var dbfavorites = await _favoritesService.GetFavoritesByUserId(dbcustomer.UserId);
            customer.Favorites = dbfavorites;
            var dbhelps = await _helpService.GetByEmailHelpAsync(dbcustomer.Email);
            customer.Helps = dbhelps;
            var orders = await _orderServices.GetOrderByUserId(dbcustomer.UserId);
            customer.Orders = orders;
            var subscribe = await _subscriberService.GetByEmailSubscriber(dbcustomer.Email);
            customer.Subscribe = subscribe;
            return View(customer);
        }
    }
}
