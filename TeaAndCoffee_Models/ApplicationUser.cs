using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaAndCoffee_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        [NotMapped]
        public string Address { get; set; }
    }
}
