using Banking.Db;
using Banking.Db.Entities;
using Banking.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests
{
    public class AccountServiceTest : TestBase
    {    
        private readonly AccountService _service;

        public AccountServiceTest() : base()
        {           
            _service = new AccountService(_context);
        }
      
        [Fact]
        public async Task CreateAccountAsync_ShouldAddAccount()
        {
            ResetDatabase();

            var initialBalance = 100;
         
            var accountId = await _service.CreateAccountAsync(initialBalance);
            var account = await _context.Accounts.FindAsync(accountId);
    
            Assert.NotNull(account);
            Assert.Equal(initialBalance, account.Balance);          
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldThrowException_ForNegativeBalance()
        {
            ResetDatabase();

            var initialBalance = -100;
          
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAccountAsync(initialBalance));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAccounts()
        {
            ResetDatabase();

            var Accounts = new List<Account>
            {
                new Account { Id = 1, Balance = 100 },
                new Account { Id = 2, Balance = 200 }
            };

            _context.Accounts.AddRange(Accounts);
            await _context.SaveChangesAsync();
         
            var result = await _service.GetAllAsync();
     
            Assert.Equal(Accounts.Count, result.Count);        
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
        {
            ResetDatabase();

            var account = new Account { Balance = 100 };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        
            var result = await _service.GetByIdAsync(account.Id);
        
            Assert.NotNull(result);
            Assert.Equal(account.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            ResetDatabase();

            var Accounts = new List<Account>
            {
                new Account { Id = 1, Balance = 100 },
                new Account { Id = 2, Balance = 200 }
            };

            _context.Accounts.AddRange(Accounts);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(3);
      
            Assert.Null(result);           
        }
    }
}
