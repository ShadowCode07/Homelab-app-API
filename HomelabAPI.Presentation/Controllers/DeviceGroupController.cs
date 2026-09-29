using HomelabAPI.Application.DTOs.DeviceGroups;
using HomelabAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomelabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DeviceGroupController : ControllerBase
    {
        private readonly IDeviceGroupService _deviceGroupService;

        public DeviceGroupController(IDeviceGroupService deviceGroupService)
        {
            _deviceGroupService = deviceGroupService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<DeviceGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDeviceGroups()
            => Ok(await _deviceGroupService.GetDeviceGroupsAsync());
    }
}
