using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;


namespace SuperAdmin.Models
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }

        public string? PackageName { get; set; }

        public string UserId { get; set; }

        public bool Active { get; set; }

        public string Duration { get; set; }

        // Start date of the package
        public DateTime StartDate { get; set; }

        // End date of the package
        public DateTime EndDate { get; set; }

        // Creation date of the package
        public DateTime CreatedOn { get; set; }

        // Price of the package
        public double Price { get; set; }
    }
}
