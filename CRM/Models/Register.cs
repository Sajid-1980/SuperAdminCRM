using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperAdmin.Models
{
    public class Register
    {
        [Key]
        public Guid ClientId { get; set; } = Guid.NewGuid();
        public string? UserClientId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string? UserName { get; set; }
       
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
