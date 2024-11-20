using Banking.Db.Entities;
using Banking.Models;
using Banking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService) 
        { 
            _accountService = accountService;
        }

        [HttpPost]        
        public async Task<AddAccountResponseDto> Add(AccountDto accountDto)
        {
            var id = await _accountService.CreateAccountAsync(accountDto.InitialBalance);

            return new AddAccountResponseDto { AccountId = id };
        }

        [HttpGet()]      
        public async Task<List<Account>> GetAllAsync()
        {
            var accounts = await _accountService.GetAllAsync();

            return accounts;
        }

        [ProducesResponseType<AddAccountResponseDto>(StatusCodes.Status200OK)]
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]long id) 
        {
            var account = await _accountService.GetByIdAsync(id);

            if (account == null)
                return NotFound();

            return Ok(account);
        }
    }
}
