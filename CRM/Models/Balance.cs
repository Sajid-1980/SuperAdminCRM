using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperAdmin.Models
{
    public class Balance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BalanceId { get; set; }

        public string UserId { get; set; }
        public string? transactiontype { get; set; }

        public string? packagename { get; set; }
        public double? previousamount { get; set; }
        public double? amount { get; set; }
        public string? AccountNumber { get; set; }

        public string? narration { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public Double BalanceAmount { get; set; }
    }
}
