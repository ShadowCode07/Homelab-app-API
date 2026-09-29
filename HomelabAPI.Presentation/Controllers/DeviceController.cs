using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Application.Exceptions;
using HomelabAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomelabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<DeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _deviceService.GetDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }


        [HttpPost]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceCreateDto dto)
        {
            try
            {
                var device = await _deviceService.CreateDeviceAsync(dto);
                return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
            }
            catch (DuplicateHostnameException ex)
            {
                ModelState.AddModelError(nameof(DeviceCreateDto.Hostname), ex.Message);
                return Conflict(new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Hostname is already in use."
                });
            }
            catch (DeviceGroupNotFoundException ex)
            {
                ModelState.AddModelError(nameof(DeviceCreateDto.DeviceGroupId), ex.Message);
                return ValidationProblem(ModelState);
            }
        }
    }
}
