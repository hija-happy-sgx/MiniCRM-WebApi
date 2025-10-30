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
    //[Authorize]
    public class CommunicationLogController : ControllerBase
    {
        private readonly CommunicationLogService _logService;
        private readonly LeadsService _leadsService;
        private readonly DealsService _dealsService;
        private readonly CrmDbContext _context;
        public CommunicationLogController(CommunicationLogService logService, LeadsService leadsService, DealsService dealsService, CrmDbContext context)
        {
            _dealsService = dealsService;
            _logService = logService;
            _leadsService = leadsService;
            _context = context;
        }

        [HttpGet("salesrep/{salesRepId}")]
        public async Task<IActionResult> GetBySalesRep(int salesRepId)
        {
            var logs = await _logService.GetBySalesRepIdAsync(salesRepId);
            return Ok(logs);
        }


        [HttpGet("my-logs/{salesRepId}")]
        public async Task<IActionResult> GetMyLogs(int salesRepId)
        {
            var logs = await _context.CommunicationLogs
                .Where(c => c.SalesRepId == salesRepId)
                .Include(c => c.Lead) // join lead info
                .Select(c => new {
                    c.LogId,
                    c.Type,
                    c.Notes,
                    c.LogDate,
                    LeadId = c.LeadId,
                    LeadName = c.Lead.Name,
                    LeadStatus = c.Lead.Status
                })
                .OrderByDescending(c => c.LogDate)
                .ToListAsync();

            return Ok(logs);
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
        public async Task<IActionResult> Add([FromBody] CommunicationLogDto dto)
        {
            var log = new CommunicationLog
            {
                Type = dto.Type,
                Notes = dto.Notes,
                LogDate = dto.LogDate,
                SalesRepId = dto.SalesRepId,
                LeadId = dto.TargetType == "Lead" ? dto.TargetId : null,
                DealId = dto.TargetType == "Deal" ? dto.TargetId : null
            };

            await _logService.AddAsync(log);

            // Update Lead or Deal status
            if (dto.TargetType == "Lead")
            {
                var leadDto = await _leadsService.GetLeadByIdAsync(dto.TargetId);
                if (leadDto.Status == LeadStatus.New)
                {
                    leadDto.Status = LeadStatus.Contacted;
                    var leadEntity = new Lead
                    {
                        LeadId = leadDto.LeadId,
                        Name = leadDto.Name,
                        Email = leadDto.Email,
                        Phone = leadDto.Phone,
                        Company = leadDto.Company,
                        Source = leadDto.Source,
                        Status = LeadStatus.Contacted,  // update status
                        AssignedToSalesRep = leadDto.AssignedToSalesRep,
                        AssignedToSrm = leadDto.AssignedToSrm,
                        CreatedByManager = leadDto.CreatedByManager,
                        CreatedAt = leadDto.CreatedAt
                    };

                    await _leadsService.UpdateLeadAsync(leadEntity);
                }
            }
            else if (dto.TargetType == "Deal")
            {
                var deal = await _dealsService.GetDealByIdAsync(dto.TargetId);
                if (deal.Status == DealStatus.Open)
                {
                    deal.Status = DealStatus.Open;
                    await _dealsService.UpdateDealAsync(deal);
                }
            }

            return Ok(log);


            //var created = await _logService.AddAsync(log);
            //return CreatedAtAction(nameof(Add), new { id = created.LogId }, created);
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
