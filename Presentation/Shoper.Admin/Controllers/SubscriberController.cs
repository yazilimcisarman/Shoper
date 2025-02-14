using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.SubscriberServices;

namespace Shoper.Admin.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly ISubscriberService _subscriberService;

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        public async Task<IActionResult> Index()
        {
            var value = await _subscriberService.GetAllSubscribers();
            return View(value);
        }
    }
}
