namespace SuperAdmin.Models
{
    public class Notification
    {

        public int Id { get; set; }
        public int clientId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        // Other relevant properties
    }
   
}
