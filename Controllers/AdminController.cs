using CRMWebApi.DTOs;
using CRMWepApi.DTOs;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only Admin can access these endpoints
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        #region USERS

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

        [HttpPost("srm/{managerId}")]
        public async Task<IActionResult> CreateSalesRepManager(int managerId, [FromBody] RegisterUserDto dto)
        {
            var srm = await _adminService.CreateSalesRepManagerAsync(dto, managerId);
            return Ok(srm);
        }

        [HttpPost("salesrep/{srmId}")]
        public async Task<IActionResult> CreateSalesRep(int srmId, [FromBody] RegisterUserDto dto)
        {
            var rep = await _adminService.CreateSalesRepAsync(dto, srmId);
            return Ok(rep);
        }

        [HttpPut("user/{role}/{id}")]
        public async Task<IActionResult> UpdateUser(string role, int id, [FromBody] RegisterUserDto dto)
        {
            await _adminService.UpdateUserAsync(id, dto, role);
            return Ok(new { message = $"{role} updated successfully" });
        }

        [HttpDelete("user/{role}/{id}")]
        public async Task<IActionResult> DeleteUser(string role, int id)
        {
            await _adminService.DeleteUserAsync(id, role);
            return Ok(new { message = $"{role} deleted successfully" });
        }

        #endregion

        #region PIPELINE STAGES

        [HttpGet("pipeline-stages")]
        public async Task<IActionResult> GetPipelineStages()
        {
            var stages = await _adminService.GetPipelineStagesAsync();
            return Ok(stages);
        }

        [HttpPost("pipeline-stage")]
        public async Task<IActionResult> AddPipelineStage([FromBody] PipelineStageDto dto)
        {
            var stage = await _adminService.AddPipelineStageAsync(dto.StageName, dto.Order);
            return Ok(stage);
        }

        #endregion
    }

   
}
