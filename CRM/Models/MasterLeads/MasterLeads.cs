using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperAdmin.Models.Masterleads
{
  
    public class Masterleads
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // This attribute auto-generates the value
        public string? Id { get; set; } // Change the data type to string

        public string? FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }


        public string? profession { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }



        public string? Country { get; set; }





        public string? Area { get; set; }



        // Add any other properties you may need...
      
    }

}
