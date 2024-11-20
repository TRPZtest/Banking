using Banking.Db.Entities;
using Banking.Db;
using Banking.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests
{
    public class TransactionServiceTest : TestBase
    {        
        [Fact]
        public async Task DepositAsync_ShouldIncreaseAccountBalance()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);
      
            var account = new Account { Id = 1, Balance = 100 };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            var depositAmount = 50;
            await service.DepositAsync(account.Id, depositAmount);

            var updatedAccount = await dbContext.Accounts.FindAsync(account.Id);
            Assert.Equal(150, updatedAccount.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldDecreaseAccountBalance()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);       

            var account = new Account { Id = 1, Balance = 100 };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            var withdrawAmount = 50;
            await service.WithdrawAsync(account.Id, withdrawAmount);

            var updatedAccount = await dbContext.Accounts.FindAsync(account.Id);
            Assert.Equal(50m, updatedAccount.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldThrowException_ForInsufficientFunds()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);

            var account = new Account { Id = 1, Balance = 50 };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentException>(() => service.WithdrawAsync(account.Id, 100));
        }

        [Fact]
        public async Task TransferAsync_ShouldTransferFundsBetweenAccounts()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);        

            var fromAccount = new Account { Id = 1, Balance = 100 };
            var toAccount = new Account { Id = 2, Balance = 50 };
            dbContext.Accounts.AddRange(fromAccount, toAccount);
            await dbContext.SaveChangesAsync();

            var transferAmount = 30;
            await service.TransferAsync(fromAccount.Id, toAccount.Id, transferAmount);

            var updatedFromAccount = await dbContext.Accounts.FindAsync(fromAccount.Id);
            var updatedToAccount = await dbContext.Accounts.FindAsync(toAccount.Id);

            Assert.Equal(70, updatedFromAccount.Balance);
            Assert.Equal(80, updatedToAccount.Balance);
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowException_ForSameSourceAndDestinationAccounts()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);          

            var account = new Account { Id = 1, Balance = 100 };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.TransferAsync(account.Id, account.Id, 10));
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowException_ForInsufficientFunds()
        {
             using var dbContext = GetDbContext();
            var service = new TransactionService(dbContext);          

            var fromAccount = new Account { Id = 1, Balance = 30 };
            var toAccount = new Account { Id = 2, Balance = 50 };
            dbContext.Accounts.AddRange(fromAccount, toAccount);
            await dbContext.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentException>(() => service.TransferAsync(fromAccount.Id, toAccount.Id, 100));
        }
    }
}