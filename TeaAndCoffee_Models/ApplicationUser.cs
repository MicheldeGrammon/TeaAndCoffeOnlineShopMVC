using Microsoft.AspNetCore.Identity;

namespace TeaAndCoffee_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
