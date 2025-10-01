using CRMWebApi.DTOs;
using CRMWepApi.DTOs;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
                       _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("manager")]
        public async Task<IActionResult> CreateManager([FromBody] RegisterUserDto dto)
        {
            var manager = await _adminService.CreateManagerAsync(dto);
            return Ok(manager);
        }

        [HttpPost("salesrepmanager")]
        public async Task<IActionResult> CreateSalesRepManager([FromQuery] int managerId, [FromBody] RegisterUserDto dto)
        {
            var srm = await _adminService.CreateSalesRepManagerAsync(dto, managerId);
            return Ok(srm);
        }

        [HttpPost("salesrep")]
        public async Task<IActionResult> CreateSalesRep([FromQuery] int srmId, [FromBody] RegisterUserDto dto)
        {
            var sr = await _adminService.CreateSalesRepAsync(dto, srmId);
            return Ok(sr);
        }

        [HttpPatch("user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromQuery] string role, [FromBody] RegisterUserDto dto)
        {
            await _adminService.UpdateUserAsync(id, dto, role);
            return NoContent();
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromQuery] string role)
        {
            await _adminService.DeleteUserAsync(id, role);
            return NoContent();
        }

        [HttpGet("pipeline")]
        public async Task<IActionResult> GetPipelineStages()
        {
            var stages = await _adminService.GetPipelineStagesAsync();
            return Ok(stages);
        }

        [HttpPost("pipeline")]
        public async Task<IActionResult> AddPipelineStage([FromBody] PipelineStageDto dto)
        {
            var stage = await _adminService.AddPipelineStageAsync(dto.StageName, dto.Order);
            return Ok(stage);
        }
    }
}
