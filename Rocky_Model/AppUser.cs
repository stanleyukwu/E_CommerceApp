using Microsoft.AspNetCore.Identity;

namespace Rocky_Utility.Models
{
    public class AppUser :IdentityUser
    {
        public string FullName { get; set; }
    }
}
