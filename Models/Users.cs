using Microsoft.AspNetCore.Identity;

namespace technicalTes_Nawatech.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfilPicture { get; set; }
    }
}
