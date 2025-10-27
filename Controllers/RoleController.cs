using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.services.interfaces;
namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }


      
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles =await _service.GetAllRoles();
                return Ok(roles);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

   
        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            try
            {
                bool result = await _service.AddRoleAsync(roleName);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string roleName)
        {
            try
            {
                 bool result = await  _service.UpdateRoleAsync(id, roleName);
                 return Ok(new { result });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role =await  _service.GetRoleById(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            try
            {
                var role =await _service.GetRoleByName(roleName);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }      
       

    }
}
