using Microsoft.AspNetCore.Mvc;

namespace Shoper.WebApp.Controllers
{
    public class AddressController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetCity() 
        {
            return Json(new {success = true});
        }
    }
}
