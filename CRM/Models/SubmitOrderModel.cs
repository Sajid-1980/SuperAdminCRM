using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SuperAdmin.Models
{
    public class SubmitOrderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generated Id
        public int Id { get; set; }

        public int clientOrderId { get; set; }
        public string UserId { get; set; }
        public string Service { get; set; }
        public string VendorName { get; set; }
        public string VendorOrderId { get; set; }
        public int Quantity { get; set; }
        public string ExpectedTime { get; set; }
        public string? PreviousQuantity { get; set; }
        public string? Message { get; set; }
    }
}
