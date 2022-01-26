using Microsoft.AspNetCore.Identity;

namespace UdemyMvcApp.Models
{
    public class AppUser :IdentityUser
    {
        public string FullName { get; set; }
    }
}
