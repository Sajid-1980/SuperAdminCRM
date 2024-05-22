using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperAdmin.Models

{
    public class MaskOrder
    {
        // Properties
        [Key] 
        public int MaskOrderId { get; set; }


        public string? SmsMaskId { get; set; }
        public string? SmsUser { get; set; }
        public string? SmsPassword { get; set; }
        public String UserId { get; set; }
        public string Optionone { get; set; }
        public string Optiontwo { get; set; }
        public string Optionthree { get; set; }
        public string? Approvedoption { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Today;

       

        // You can add additional constructors if needed

        // Methods
        // You can add methods related to MaskOrder here
    }
}
