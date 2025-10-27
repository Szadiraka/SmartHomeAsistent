using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserDeviceController : ControllerBase
    {
       private readonly IUserDeviceService _service;
        public UserDeviceController(IUserDeviceService userDeviceService)
        {
           _service = userDeviceService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUserDevice([FromBody] UserDeviceDTO userDeviceDTO)
        {
            try
            {
                bool result = await _service.AddUserDevice(userDeviceDTO);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{userId:int}/{deviceId:int}")]
        public async Task<IActionResult> UpdateUserDevice( int userId, int deviceId, [FromBody] UserDeviceDTO userDeviceDTO)
        {
            try
            {
                bool result =await _service.UpdateUserDevice(userId, deviceId, userDeviceDTO);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{userId:int}/{deviceId:int}")]
        public async Task<IActionResult> DeleteUserDevice(int userId, int deviceId)
        {
            try
            {
                bool result = await _service.DeleteUserDevice(userId, deviceId);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("{userId:int}/{deviceId:int}")]
        public  async Task<IActionResult> GetUserDeviceById(int userId, int deviceId)
        {
            try
            {
                var userDevice = await _service.GetUserDeviceById(userId, deviceId);
                return Ok(userDevice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("byUser/{userId:int}")]
        public async Task<IActionResult> GetUserDevicesByUserId(int userId)
        {
            try
            {
                var userDevices = await _service.GetUserDevicesByUserId(userId);
                return Ok(userDevices);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("byDevice/{deviceId:int}")]
        public async Task<IActionResult> GetUserDevicesByDeviceId(int deviceId)
        {
            try
            {
                var userDevices = await _service.GetUserDevicesByDeviceId(deviceId);
                return Ok(userDevices);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
