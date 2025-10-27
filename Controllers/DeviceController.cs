using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                bool result = await _service.AddDeviceAsync(deviceDto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDto)
        {
            try
            {
                var result = await _service.UpdateDeviceAsync(id, deviceDto);
                return Ok(new { result });
            }
            catch(Exception ex)
            {
                    return BadRequest(new { message = ex.Message });
            }
          
           
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            try
            {
                Device device = await _service.GetDeviceById(id);
                return Ok(device);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("byAccount/{accountId:int}")]
        public async Task<IActionResult> GetDevicesByAccountId(int accountId)
        {
            try
            {
                var devices = await _service.GetDevicesByAccountId(accountId);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices([FromQuery] string? name)
        {
            try
            {
                Expression<Func<Device,bool>> filter = x =>
                (string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower()));
                var devices = await _service.GetAllDevices(filter);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }      


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                bool result = await _service.DeleteDeviceAsync(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

       
    }
}
