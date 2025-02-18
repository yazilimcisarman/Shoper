using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Dtos.EMailDtos;
using Shoper.Application.Usecasess.EmailServices;

namespace Shoper.Admin.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index(string email)
        {
            var dto = new SendEmailDto
            {
                ReciverEmail = email
            };
            return View(dto);
        }
        [HttpPost]
        public IActionResult Send(SendEmailDto dto)
        {
            var result = _emailService.SendEmailAsync(dto);
            if (result)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index");
        }
    }
}
