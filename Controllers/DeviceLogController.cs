using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                bool result = await _service.AddDeviceLog(deviceLogDTO);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDeviceLog(int id)
        {
            try
            {
                bool result = await _service.DeleteDeviceLog(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDeviceLog(int id, [FromBody] DeviceLogDTO deviceLogDTO)
        {
            try
            {
                bool result = await _service.UpdateDeviceLog(id, deviceLogDTO);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("{id:int}")]
        public async Task< IActionResult> GetDeviceLogById(int id)
        {
            try
            {
                DeviceLog deviceLog = await _service.GetDeviceLogById(id);
                return Ok(deviceLog);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("byDevice/{deviceId:int}")]

        public async Task< IActionResult> GetDeviceLogsByDeviceId(int deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            try
            {
                var deviceLogs = await _service.GetDevicesLogsByDeviceId(deviceId, from, to );
                return Ok(deviceLogs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDeviceLogs([FromQuery] DateTime? from,[FromQuery] DateTime? to)
        {
            try
            {
                var deviceLogs = await _service.GetAllDeviceLogs(from, to);
                return Ok(deviceLogs);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
