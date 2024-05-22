using SuperAdmin.Areas.Identity.Data;

namespace SuperAdmin.ViewModels.Common
{
    public class UserProfileViewModel
    {
        public ChangePasswordViewModel ChangePasswordModel { get; set; }
        public SuperAdminUser SuperAdminUserModel { get; set; }
        public IFormFile ProfileImage { get; set; }

    }
}
