using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperAdmin.Models
{
    public class SmsBalancecs
    {
        [Key]
        public int SmsBalancecsId { get; set; }

        public string UserId { get; set; }
        public Double Price { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Today;
        public bool IsActive { get; set; }
        public Double Sms { get; set; }

    }
}
