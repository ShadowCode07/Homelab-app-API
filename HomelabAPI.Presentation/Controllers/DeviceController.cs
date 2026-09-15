using Microsoft.AspNetCore.Mvc;

using HomelabAPI.Application.Interfaces;

namespace HomelabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceController;

        public DeviceController(IDeviceService deviceController)
        {
            _deviceController = deviceController;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _deviceController.GetDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var device = await _deviceController.GetDeviceByIdAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }
    }
}
