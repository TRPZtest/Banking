using Banking.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Db
{
    public class BankingDbContext : DbContext
    {
        public DbSet<Account> accounts;
    }
}
