using Banking.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests
{
    public abstract class TestBase 
    {
        protected BankingDbContext _context ;
 
        public BankingDbContext GetDbContext()
        {            
            var guid = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<BankingDbContext>()
               .UseInMemoryDatabase("BankingTestDb"+guid)
               .Options;

            var context = new BankingDbContext(options);              

            return context;
        }
    }
}
