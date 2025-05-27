using Microsoft.AspNetCore.Http;

namespace technicalTes_Nawatech.ViewModels
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }
        public string? ExistingProfilePicture { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
