namespace SuperAdmin.Models
{
    public class Invoice
    {
        // Property to store the invoice ID
        public int InvoiceId { get; set; }

        // Property to store the user ID associated with the invoice
        public string UserId { get; set; }
        public string? Status { get; set; }

        // Property to store the image of the invoice as binary data
        public byte[] ImageData { get; set; }
        
    }
}
