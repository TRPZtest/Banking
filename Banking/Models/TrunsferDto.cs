using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class TrunsferDto
    {
        [Required]
        public long FromAccountId { get; set; }
        [Required]
        public long ToAccountId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public Decimal Amount { get; set; }
    }
}
