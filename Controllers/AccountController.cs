using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

      
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountDTO accountDto)
        {        
            if (!ModelState.IsValid)
                throw new ValidationException("Данные не валидны");
            bool result= await _service.AddAccountAsync(accountDto);
            return Ok(new
            {
                success = true,
                data = result
            });          
           
           
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAccount( int id, [FromBody] AccountDTO accountDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Данные не валидны");
            bool result = await _service.UpdateAccountAsync(id, accountDto);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
             var result =await _service.GetAccountById(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var result = await _service.GetAllAccounts();
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetAllAccountsByUserId(int userId)
        {
        
            var result = await _service.GetAccountsByUserId(userId);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {        
            var result = await _service.DeleteAccountAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpPost("adduser")]  
        public async Task<IActionResult> AddSecondaryUserToAccount([FromBody] SecondaryUserDTO secondaryUserDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Данные не валидны");
            var result = await _service.AddSecondaryUserToAccountAsync(secondaryUserDto);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        


       
    }
}
