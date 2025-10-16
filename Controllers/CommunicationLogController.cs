using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CommunicationLogController : ControllerBase
    {
        private readonly CommunicationLogService _logService;

        public CommunicationLogController(CommunicationLogService logService)
        {
            _logService = logService;
        }

        // Get communications for a lead
        [HttpGet("lead/{leadId}")]
        public async Task<IActionResult> GetByLead(int leadId)
        {
            var logs = await _logService.GetByLeadIdAsync(leadId);
            return Ok(logs);
        }

        // Get communications for a deal
        [HttpGet("deal/{dealId}")]
        public async Task<IActionResult> GetByDeal(int dealId)
        {
            var logs = await _logService.GetByDealIdAsync(dealId);
            return Ok(logs);
        }

        // Add a new communication log
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CommunicationLog log)
        {
            var created = await _logService.AddAsync(log);
            return CreatedAtAction(nameof(Add), new { id = created.LogId }, created);
        }

        // Optional: Update log
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommunicationLog log)
        {
            if (id != log.LogId) return BadRequest();

            var updated = await _logService.UpdateAsync(log);
            if (!updated) return NotFound();

            return NoContent();
        }

        // Optional: Delete log
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _logService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }


    }
}
