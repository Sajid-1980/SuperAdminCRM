using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperAdmin.Models
{
    public class DetuctionType
    {
        [Key]
        public int DetuctionTypeId { get; set; }
        public int Detuction { get; set; }
    }
}
