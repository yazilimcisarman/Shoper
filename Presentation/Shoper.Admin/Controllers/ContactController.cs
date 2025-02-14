using Microsoft.AspNetCore.Mvc;
using Shoper.Application.Usecasess.ContactServices;

namespace Shoper.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var contact = await _contactService.GetAllContactAsync();
            return View(contact);
        }
        public async Task<IActionResult> Detail(int contactId)
        {
            var contact = await _contactService.GetByIdContactAsync(contactId);
            return View(contact);
        }
        public  async Task<IActionResult> Delete(int contactId)
        {
            await _contactService.DeleteContactAsync(contactId);
            return RedirectToAction("Index");
        }
    }
}
