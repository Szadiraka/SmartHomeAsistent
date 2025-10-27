using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Некорректные данные");
                bool result= await _service.AddAccountAsync(accountDto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAccount( int id, [FromBody] AccountDTO accountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Некорректные данные");
                bool result = await _service.UpdateAccountAsync(id, accountDto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var account =await _service.GetAccountById(id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _service.GetAllAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetAllAccountsByUserId(int userId)
        {
            try
            {
                var accounts = await _service.GetAccountsByUserId(userId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpDelete("id:int")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                bool result = await _service.DeleteAccountAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("adduser")]  
        public async Task<IActionResult> AddSecondaryUserToAccount([FromBody] SecondaryUserDTO secondaryUserDto)
        {
            try
            {
                bool result = await _service.AddSecondaryUserToAccountAsync(secondaryUserDto);
                return Ok(new { result });
            }catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        


       
    }
}
