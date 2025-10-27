using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Linq.Expressions;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDevice([FromBody] DeviceDTO deviceDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            var result = await _service.AddDeviceAsync(deviceDto);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDto)
        {
            if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
           
            var result = await _service.UpdateDeviceAsync(id, deviceDto);
            return Ok(new
            {
                success = true,
                data = result
            });


        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
           
            var result = await _service.GetDeviceById(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet("byAccount/{accountId:int}")]
        public async Task<IActionResult> GetDevicesByAccountId(int accountId)
        {
           
            var result = await _service.GetDevicesByAccountId(accountId);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices([FromQuery] string? name)
        {
           
            Expression<Func<Device,bool>> filter = x =>
            (string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower()));
            var result = await _service.GetAllDevices(filter);
            return Ok(new
            {
                success = true,
                data = result
            });

        }      


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
          
            var result = await _service.DeleteDeviceAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

       
    }
}
