using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class AccountDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Balance must be greater than zero.")]
        public decimal InitialBalance { get; set; }
    }
}
