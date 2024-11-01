using Microsoft.AspNetCore.Mvc;

namespace Shoper.WebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
