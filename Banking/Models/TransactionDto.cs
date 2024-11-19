using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class TransactionDto
    {
        [Required]
        public long AccountId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public Decimal Amount { get; set; }      
    }
}
