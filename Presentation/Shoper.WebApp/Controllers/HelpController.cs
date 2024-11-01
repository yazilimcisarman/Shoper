using Microsoft.AspNetCore.Mvc;

namespace Shoper.WebApp.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
