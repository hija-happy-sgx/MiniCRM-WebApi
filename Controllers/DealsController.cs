using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DealsController : ControllerBase
    {
        private readonly DealsService _dealsService;
        private readonly CommunicationLogService _logService;

        public DealsController(DealsService dealsService)
        {
            _dealsService = dealsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeals()
        {
            var deals = await _dealsService.GetAllDealsAsync();
            return Ok(deals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeal(int id)
        {
            var deal = await _dealsService.GetDealByIdAsync(id);
            if (deal == null) return NotFound();
            return Ok(deal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeal([FromBody] Deal deal)
        {
            var created = await _dealsService.CreateDealAsync(deal);
            return CreatedAtAction(nameof(GetDeal), new { id = created.DealId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeal(int id, [FromBody] Deal deal)
        {
            if (id != deal.DealId) return BadRequest();

            var updated = await _dealsService.UpdateDealAsync(deal);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeal(int id)
        {
            var deleted = await _dealsService.DeleteDealAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/communications")]
        public async Task<IActionResult> GetDealCommunications(int id)
        {
            var logs = await _logService.GetByDealIdAsync(id);
            return Ok(logs);
        }
    }
}
