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
        protected readonly BankingDbContext _context;

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<BankingDbContext>()
               .UseInMemoryDatabase("BankingTestDb")
               .Options;
            _context = new BankingDbContext(options); 
        }

        public void ResetDatabase()
        {           
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();        
        }
    }
}
