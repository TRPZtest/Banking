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
            var options = new DbContextOptionsBuilder<BankingDbContext>()
                .UseInMemoryDatabase("BankingTestDb") // Same database name for all tests
                .Options;

            _context.Database.EnsureDeleted(); // Deletes all data and schema
            _context.Database.EnsureCreated(); // Recreates the schema         
        }
    }
}
