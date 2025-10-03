using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineController : ControllerBase
    {
        private readonly PipelineService _pipelineService;

        public PipelineController(PipelineService pipelineService)
        {
            _pipelineService = pipelineService;
        }

        // GET: /api/pipeline
        // Returns pipelines visible to the logged-in user
        [HttpGet]
        [Authorize] // All logged-in users can access
        public async Task<IActionResult> GetPipelines()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var pipelines = await _pipelineService.GetPipelinesForUserAsync(role, userId);
            return Ok(pipelines);
        }

        // POST: /api/pipeline
        // Only Admin can create new pipeline stage
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStage([FromBody] PipelineStage stage)
        {
            var createdStage = await _pipelineService.CreateStageAsync(stage);
            return CreatedAtAction(nameof(GetPipelines), new { id = createdStage.StageId }, createdStage);
        }

        // PATCH: /api/pipeline/:id
        // Only Admin can update stage
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStage(int id, [FromBody] PipelineStage stage)
        {
            var updatedStage = await _pipelineService.UpdateStageAsync(id, stage);
            if (updatedStage == null) return NotFound();
            return Ok(updatedStage);
        }

        // DELETE: /api/pipeline/:id
        // Only Admin can delete stage
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStage(int id)
        {
            var deleted = await _pipelineService.DeleteStageAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
