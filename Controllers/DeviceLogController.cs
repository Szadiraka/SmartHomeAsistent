using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceLogController : ControllerBase
    {
        private readonly IDeviceLogService _service;

        public DeviceLogController(IDeviceLogService service)
        {
            _service = service;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddDeviceLog([FromBody] DeviceLogDTO deviceLogDTO)
        {
           if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            var result = await _service.AddDeviceLog(deviceLogDTO);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDeviceLog(int id)
        {
          
            var result = await _service.DeleteDeviceLog(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDeviceLog(int id, [FromBody] DeviceLogDTO deviceLogDTO)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");

            var result = await _service.UpdateDeviceLog(id, deviceLogDTO);
            return Ok(new
            {
                success = true,
                data = result
            });

        }



        [HttpGet("{id:int}")]
        public async Task< IActionResult> GetDeviceLogById(int id)
        {
           
            var result = await _service.GetDeviceLogById(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpGet("byDevice/{deviceId:int}")]

        public async Task< IActionResult> GetDeviceLogsByDeviceId(int deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
           
            var result = await _service.GetDevicesLogsByDeviceId(deviceId, from, to );
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpGet]
        public async Task<IActionResult> GetAllDeviceLogs([FromQuery] DateTime? from,[FromQuery] DateTime? to)
        {
           
            var result = await _service.GetAllDeviceLogs(from, to);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

    }
}
