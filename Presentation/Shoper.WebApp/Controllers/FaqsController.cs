using Microsoft.AspNetCore.Mvc;

namespace Shoper.WebApp.Controllers
{
    public class FaqsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
