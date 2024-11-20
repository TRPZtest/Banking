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
        private readonly TransactionService _service;

        public TransactionServiceTest() : base()
        {
            _service = new TransactionService(_context);
        }

        [Fact]
        public async Task DepositAsync_ShouldIncreaseAccountBalance()
        {
            ResetDatabase();

            var account = new Account { Id = 1, Balance = 100 };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var depositAmount = 50;

            await _service.DepositAsync(account.Id, depositAmount);

            var updatedAccount = await _context.Accounts.FindAsync(account.Id);
            Assert.Equal(150, updatedAccount.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldDecreaseAccountBalance()
        {
            ResetDatabase();

            var account = new Account { Id = 1, Balance = 100 };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var withdrawAmount = 50;

            await _service.WithdrawAsync(account.Id, withdrawAmount);

            var updatedAccount = await _context.Accounts.FindAsync(account.Id);
            Assert.Equal(50m, updatedAccount.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldThrowException_ForInsufficientFunds()
        {
            ResetDatabase();

            var account = new Account { Id = 1, Balance = 50 };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentException>(() => _service.WithdrawAsync(account.Id, 100));
        }

        [Fact]
        public async Task TransferAsync_ShouldTransferFundsBetweenAccounts()
        {
            ResetDatabase();

            var fromAccount = new Account { Id = 1, Balance = 100 };
            var toAccount = new Account { Id = 2, Balance = 50 };
            _context.Accounts.AddRange(fromAccount, toAccount);
            await _context.SaveChangesAsync();

            var transferAmount = 30;

            await _service.TransferAsync(fromAccount.Id, toAccount.Id, transferAmount);

            var updatedFromAccount = await _context.Accounts.FindAsync(fromAccount.Id);
            var updatedToAccount = await _context.Accounts.FindAsync(toAccount.Id);

            Assert.Equal(70, updatedFromAccount.Balance);
            Assert.Equal(80, updatedToAccount.Balance);
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowException_ForSameSourceAndDestinationAccounts()
        {
            ResetDatabase();

            var account = new Account { Id = 1, Balance = 100 };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.TransferAsync(account.Id, account.Id, 10));
        }

        [Fact]
        public async Task TransferAsync_ShouldThrowException_ForInsufficientFunds()
        {
            ResetDatabase();

            var fromAccount = new Account { Id = 1, Balance = 30 };
            var toAccount = new Account { Id = 2, Balance = 50 };
            _context.Accounts.AddRange(fromAccount, toAccount);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentException>(() => _service.TransferAsync(fromAccount.Id, toAccount.Id, 100));
        }
    }
}