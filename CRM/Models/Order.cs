using System.ComponentModel.DataAnnotations;
namespace SuperAdmin.Models

{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string userId { get; set; }

        [Required]
        public string Service { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string AverageTime { get; set; }

        [Required]
        public string OrderStatus { get; set; }
        public string Link { get; set; }
        public string? PreviousQuantity { get; set; }
        public string? Message { get; set; }
    }
}
