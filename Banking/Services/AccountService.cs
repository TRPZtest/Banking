using Banking.Db;
using Banking.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Banking.Services
{  
    public class AccountService 
    {
        private readonly BankingDbContext _context;

        public AccountService(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAccountAsync(decimal initialBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            var account = new Account { Balance = initialBalance };

            _context.accounts.Add(account);

            await _context.SaveChangesAsync();

            return account.Id;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            var accounts = await _context.accounts.ToListAsync();

            return accounts;
        }

        public async Task<Account?> GetByIdAsync(long id)
        {
            var account = await _context.accounts.SingleOrDefaultAsync(x => x.Id == id);

            return account;
        }
    }
}
