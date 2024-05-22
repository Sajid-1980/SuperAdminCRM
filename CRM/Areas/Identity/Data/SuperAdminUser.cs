using Microsoft.AspNetCore.Identity;

namespace SuperAdmin.Areas.Identity.Data
{
    public class SuperAdminUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string? Notes { get; set; }
        public string? Department { get; set; }
        public string? TeamMemeberType { get; set; }
        public string? Notification { get; set; }
        public string? ProfileImage { get; set; }

    }
}
