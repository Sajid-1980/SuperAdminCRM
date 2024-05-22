using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperAdmin.Models
{
    public class LeadRate
    {
        [Key]
        public int LeadRateId { get; set; }
        public Double UnitLead { get; set; }
    }
}
