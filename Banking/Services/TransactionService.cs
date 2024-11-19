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

        // Deposit funds
        public async Task DepositAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            var account = await _context.accounts.SingleOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {accountId} not found.");

            account.Balance += amount;

            await _context.SaveChangesAsync();
        }

        // Withdraw funds
        public async Task WithdrawAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            var account = await _context.accounts.SingleOrDefaultAsync(x => x.Id == accountId);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {accountId} not found.");

            if (account.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            account.Balance -= amount;

            await _context.SaveChangesAsync();
        }

        // Transfer funds
        public async Task TransferAsync(long fromAccountId, long toAccountId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero.");
            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer to the same account.");

            var fromAccount = await _context.accounts.SingleOrDefaultAsync(x => x.Id == fromAccountId);
            var toAccount = await _context.accounts.SingleOrDefaultAsync(x => x.Id == toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new InvalidOperationException("One or both accounts do not exist.");

            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient funds in the source account.");

            // Perform the transfer
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            await _context.SaveChangesAsync();
        }
    }
}
