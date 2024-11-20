using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Db.Entities
{
    public class Account
    {
        [Key]
        public long Id { get; set; }
        [Precision(14, 2)]
        [Required]
        public decimal Balance { get; set; }
    }
}
