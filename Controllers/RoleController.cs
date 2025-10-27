using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
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
           
            var result =await _service.GetAllRoles();
            return Ok(new
            {
                success = true,
                data = result
            });


        }

   
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            if(string.IsNullOrWhiteSpace(roleName))
                throw new ValidationException("Некорректные данные");
            
            var result = await _service.AddRoleAsync(roleName);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ValidationException("Некорректные данные");

            var result = await  _service.UpdateRoleAsync(id, roleName);
            return Ok(new
            {
                success = true,
                data = result
            });

        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
           
              var result =await  _service.GetRoleById(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
           
            var result =await _service.GetRoleByName(roleName);
            return Ok(new
            {
                success = true,
                data = result
            });

        }      
       

    }
}
