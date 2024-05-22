namespace SuperAdmin.Models
{
    public class Notificationscs
    {

        public int Id { get; set; }
        public string clientId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        // Other relevant properties
    }
   
}
