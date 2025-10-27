using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;
using SmartHomeAsistent.services.interfaces;


namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;


        public UserController(IUserService userService)
        {
            _service = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            {
                User user = await _service.RegisterAsync(userDto);

                return Ok(new { user.Id, user.Email, roleName = user.Role.Name });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                string token =await _service.Login(loginDto.Email, loginDto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromBody] UserFilter? filter)
        {
            try
            {
                var users = await _service.GetAllUsersAsync(filter);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task< IActionResult> GetById(int id)
        {
            try
            {
                var user =await _service.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDto)
        {
           
            try
            {
                bool result = await _service.UpdateUserAsync(id, userDto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            try
            {
                bool result = await _service.BlockUserAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser(int id)
        {
            try
            {
                bool result = await _service.UnblockUserAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet("delete{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result = await _service.DeleteUserAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("recovery/{id}")]
        public async Task<IActionResult> RecoveryUser(int id)
        {
            try
            {
                bool result = await _service.RecoveryUserAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
