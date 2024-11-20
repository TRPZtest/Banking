using Banking.Db;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services
{
    public class TransactionService
    {
        private readonly BankingDbContext _context;

        public TransactionService(BankingDbContext context)
        {
            _context = context;
        }
       
        public async Task DepositAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Deposit amount must be greater than zero.");

            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
                throw new ArgumentException($"Account with ID {accountId} not found.");

            account.Balance += amount;

            await _context.SaveChangesAsync();
        }
       
        public async Task WithdrawAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawal amount must be greater than zero.");

            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
                throw new ArgumentException($"Account with ID {accountId} not found.");

            if (account.Balance < amount)
                throw new ArgumentException("Insufficient funds.");

            account.Balance -= amount;

            await _context.SaveChangesAsync();
        }

        public async Task TransferAsync(long fromAccountId, long toAccountId, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Transfer amount must be greater than zero.");
            if (fromAccountId == toAccountId)
                throw new InvalidOperationException("Cannot transfer to the same account.");

            var fromAccount = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == fromAccountId);
            var toAccount = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("One or both Accounts do not exist.");

            if (fromAccount.Balance < amount)
                throw new ArgumentException("Insufficient funds in the source account.");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            await _context.SaveChangesAsync();
        }
    }
}
