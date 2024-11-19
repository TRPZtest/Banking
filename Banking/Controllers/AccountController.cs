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
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService) 
        { 
            _accountService = accountService;
        }

        [HttpPost]        
        public async Task<AddAccountResponseDto> Add(AccountDto accountDto)
        {
            var id = await _accountService.CreateAccount(accountDto.Balance);

            return new AddAccountResponseDto { AccountId = id };
        }

        [HttpGet("{id}")]      
        public async Task<List<Account>> GetAllAsync()
        {
            var accounts = await _accountService.GetAll();

            return accounts;
        }

        [ProducesResponseType<AddAccountResponseDto>(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(long id) 
        {
            var account = _accountService.GetById(id);

            if (account == null)
                return NotFound();

            return Json(account);
        }

    }
}
