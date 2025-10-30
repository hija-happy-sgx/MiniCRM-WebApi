using CRMWebApi.DTOs;
using CRMWepApi.Data;
using CRMWepApi.DTOs;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DealsController : ControllerBase
    {
        private readonly DealsService _dealsService;
        private readonly CommunicationLogService _logService;
        private readonly CrmDbContext _context;

        public DealsController(DealsService dealsService, CommunicationLogService logService, CrmDbContext context)
        {
            _context = context;
            _dealsService = dealsService;
            _logService = logService;

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

        //[HttpPost]
        //public async Task<IActionResult> CreateDeal([FromBody] Deal deal)
        //{
        //    var created = await _dealsService.CreateDealAsync(deal);
        //    return CreatedAtAction(nameof(GetDeal), new { id = created.DealId }, created);
        //}



        [HttpPost("deals")]
        public async Task<IActionResult> CreateDeal([FromQuery] int salesRepId, [FromBody] CreateDealDto model)
        {
            var lead = await _context.Leads.FirstOrDefaultAsync(l => l.LeadId == model.LeadId);
            if (lead == null || lead.Status != LeadStatus.Qualified)
                return BadRequest("Lead is not qualified or doesn't exist.");
            if (lead.Status != LeadStatus.Qualified)
                return BadRequest("Only Qualified leads can be converted to deals.");

            var firstStage = await _context.PipelineStages.OrderBy(s => s.StageOrder).FirstOrDefaultAsync();

            var deal = new Deal
            {
                LeadId = lead.LeadId,
                AssignedToSalesRep = salesRepId,
                DealName = model.Name,
                Value = model.Value,
                StageId = model.StageId,
                Status = DealStatus.Open,
                CreatedAt = DateTime.UtcNow,
                ExpectedCloseDate = model.ExpectedCloseDate
            };

            _context.Deals.Add(deal);

            // to convert the lead
            lead.Status = LeadStatus.Converted;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Deal created successfully.", DealId = deal.DealId });
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
