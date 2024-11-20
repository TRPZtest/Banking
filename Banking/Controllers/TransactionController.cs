using Banking.Db.Entities;
using Banking.Models;
using Banking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Banking.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : TransactionControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost()]
        public async Task<IActionResult> Deposit(TransactionDto transactionDto)
        {
            return await ExecuteTransaction(async () =>
            {
                await _transactionService.DepositAsync(transactionDto.AccountId, transactionDto.Amount);
                return Ok(new { message = "Deposit successful." });
            });
        }

        [HttpPost()]
        public async Task<IActionResult> Withdraw(TransactionDto transactionDto)
        {
            return await ExecuteTransaction(async () =>
            {
                await _transactionService.WithdrawAsync(transactionDto.AccountId, transactionDto.Amount);
                return Ok(new { message = "Withdrawal successful." });
            });
        }

        [HttpPost()]
        public async Task<IActionResult> Transfer(TrunsferDto transferDto)
        {
            return await ExecuteTransaction(async () =>
            {
                await _transactionService.TransferAsync(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount);
                return Ok(new { message = "Transfer successful." });
            });
        }           
    }
}
