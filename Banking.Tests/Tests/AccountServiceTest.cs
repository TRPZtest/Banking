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
        [Fact]
        public async Task CreateAccountAsync_ShouldAddAccount()
        {
            using var dbContext = GetDbContext();
            var service = new AccountService(dbContext);

            var initialBalance = 100;
            var accountId = await service.CreateAccountAsync(initialBalance);
            var account = await dbContext.Accounts.FindAsync(accountId);

            Assert.NotNull(account);
            Assert.Equal(initialBalance, account.Balance);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldThrowException_ForNegativeBalance()
        {
             using var dbContext = GetDbContext();
            var service = new AccountService(dbContext);

            var initialBalance = -100;
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAccountAsync(initialBalance));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAccounts()
        {
             using var dbContext = GetDbContext();
            var service = new AccountService(dbContext);

            var accounts = new List<Account>
        {
            new Account { Id = 1, Balance = 100 },
            new Account { Id = 2, Balance = 200 }
        };

            dbContext.Accounts.AddRange(accounts);
            await dbContext.SaveChangesAsync();

            var result = await service.GetAllAsync();

            Assert.Equal(accounts.Count, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
        {
             using var dbContext = GetDbContext();
            var service = new AccountService(dbContext);

            var account = new Account { Balance = 100 };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            var result = await service.GetByIdAsync(account.Id);

            Assert.NotNull(result);
            Assert.Equal(account.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
        {
             using var dbContext = GetDbContext();
            var service = new AccountService(dbContext);

            var accounts = new List<Account>
        {
            new Account { Id = 1, Balance = 100 },
            new Account { Id = 2, Balance = 200 }
        };

            dbContext.Accounts.AddRange(accounts);
            await dbContext.SaveChangesAsync();

            var result = await service.GetByIdAsync(3);

            Assert.Null(result);
        }
    }
}
