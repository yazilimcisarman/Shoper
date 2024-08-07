using Microsoft.AspNetCore.Identity;

namespace Shoper.Persistence.Context.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
