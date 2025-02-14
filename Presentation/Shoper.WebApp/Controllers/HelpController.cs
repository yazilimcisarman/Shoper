using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.HelpDtos;
using Shoper.Application.Usecasess.HelpServices;

namespace Shoper.WebApp.Controllers
{
    public class HelpController : Controller
    {
        private readonly IHelpService _services;

        public HelpController(Application.Usecasess.HelpServices.IHelpService services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateHelp(CreateHelpDto dto)
        {
            dto.CreatedDate=DateTime.Now;
            dto.Status = 0;
            await _services.CreateHelpAsync(dto);
            return RedirectToAction("Index");
        }
    }
}
