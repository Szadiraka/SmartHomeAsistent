using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartHomeAsistent.CustomExceptions;
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

        [HttpPost]
        public async Task<IActionResult> AddUserDevice([FromBody] UserDeviceDTO userDeviceDTO)
        {
           if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            bool result = await _service.AddUserDevice(userDeviceDTO);
            return Ok(new { result });
           
        }


        [HttpPut("{userId:int}/{deviceId:int}")]
        public async Task<IActionResult> UpdateUserDevice( int userId, int deviceId, [FromBody] UserDeviceDTO userDeviceDTO)
        {
           
                if(!ModelState.IsValid)
                    throw new ValidationException("Некорректные данные");

            bool result =await _service.UpdateUserDevice(userId, deviceId, userDeviceDTO);
            return Ok(new { result });
           
        }


        [HttpDelete("{userId:int}/{deviceId:int}")]
        public async Task<IActionResult> DeleteUserDevice(int userId, int deviceId)
        {
           
            bool result = await _service.DeleteUserDevice(userId, deviceId);
            return Ok(new { result });
            

        }

        [HttpGet("{userId:int}/{deviceId:int}")]
        public  async Task<IActionResult> GetUserDeviceById(int userId, int deviceId)
        {
           
            var userDevice = await _service.GetUserDeviceById(userId, deviceId);
            return Ok(userDevice);
           
        }


        [HttpGet("byUser/{userId:int}")]
        public async Task<IActionResult> GetUserDevicesByUserId(int userId)
        {
           
            var userDevices = await _service.GetUserDevicesByUserId(userId);
            return Ok(userDevices);
           
        }

        [HttpGet("byDevice/{deviceId:int}")]
        public async Task<IActionResult> GetUserDevicesByDeviceId(int deviceId)
        {
           
            var userDevices = await _service.GetUserDevicesByDeviceId(deviceId);
            return Ok(userDevices);
           
        }



    }
}
