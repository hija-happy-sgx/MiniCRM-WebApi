using CRMWebApi.DTOs;
using CRMWepApi.Data;
using CRMWepApi.DTOs;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeadsController : ControllerBase
    {
        private readonly LeadsService _leadsService;
        private readonly CommunicationLogService _logService;
        private readonly CrmDbContext _context;


        public LeadsController(LeadsService leadsService, CommunicationLogService logService, CrmDbContext context)
        {
           
                _leadsService = leadsService;
            _logService = logService;
            _context = context;
        }

        [HttpGet("summary/{salesRepId}")]
        public async Task<ActionResult<LeadDashboardDto>> GetLeadSummary(int salesRepId)
        {
            var totalLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId)
                .CountAsync();

            var newLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.New)
                .CountAsync();

            var contactedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Contacted)
                .CountAsync();

            var convertedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Converted)
                .CountAsync();

            var disqualifiedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Disqualified)
                .CountAsync();
            var qualifiedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Qualified)
                .CountAsync();

            var dashboard = new LeadDashboardDto
            {
                TotalLeads = totalLeads,
                NewLeads = newLeads,
                ContactedLeads = contactedLeads,
                ConvertedLeads = convertedLeads,
                DisqualifiedLeads = disqualifiedLeads,
                QualifiedLeads = qualifiedLeads
            };

            return Ok(dashboard);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllLeads()
        //{
        //    var leads = await _leadsService.GetAllLeadsAsync();
        //    return Ok(leads);
        //}


        [HttpGet("leads")]
        public async Task<IActionResult> GetLeads(int salesRepId)
        {
            var leads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId)
                .Select(l => new { l.LeadId, l.Name, l.Status })
                .ToListAsync();

            return Ok(leads);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllLeads([FromQuery] int? salesRepId)
        //{
        //    IEnumerable<LeadDto> leadsDto;

        //    if (salesRepId.HasValue)
        //        leadsDto = await _leadsService.GetLeadsBySalesRepAsync(salesRepId.Value);
        //    else
        //        leadsDto = await _leadsService.GetAllLeadsAsync();

        //    return Ok(leadsDto);
        //}


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLead(int id)
        {
            var lead = await _leadsService.GetLeadByIdAsync(id);
            if (lead == null) return NotFound();
            return Ok(lead);
        }



        //[HttpPost]
        //public async Task<IActionResult> CreateLead([FromBody] LeadDto dto)
        //{
        //    var lead = new Lead
        //    {
        //        Name = dto.Name,
        //        Email = dto.Email,
        //        Phone = dto.Phone,
        //        Company = dto.Company,
        //        Source = dto.Source,
        //        Status = dto.Status ?? LeadStatus.New,

        //        //            Status = Enum.TryParse<LeadStatus>(dto.Status, true, out var parsedStatus)
        //        //? parsedStatus
        //        //: LeadStatus.New;


        //        AssignedToSalesRep = dto.AssignedToSalesRep,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow
        //    };

        //    var created = await _leadsService.CreateLeadAsync(lead);
        //    return CreatedAtAction(nameof(GetLead), new { id = created.LeadId }, created);
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(int id, [FromBody] Lead lead)
        {
            if (id != lead.LeadId) return BadRequest();

            var updated = await _leadsService.UpdateLeadAsync(lead);
            if (!updated) return NotFound();

            return NoContent();
        }

        //[HttpPatch("{leadId}")]
        //public async Task<IActionResult> UpdateLeadStatus(int leadId, [FromBody] LeadStatusUpdateModel model)
        //{
        //    var lead = await _context.Leads.FindAsync(leadId);
        //    if (lead == null) return NotFound();

        //    lead.Status = model.Status;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        [HttpPatch("qualify/{leadId}")]
        public async Task<IActionResult> QualifyLead(int leadId)
        {
            var lead = await _context.Leads.FindAsync(leadId);
            if (lead == null)
                return NotFound("Lead not found.");

            // Only allow qualification from Contacted state
            if (lead.Status != LeadStatus.Contacted)
                return BadRequest("Only leads in 'Contacted' status can be qualified.");

            lead.Status = LeadStatus.Qualified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Lead successfully qualified.", LeadId = leadId });
        }

        [HttpPatch("disqualify/{leadId}")]
        public async Task<IActionResult> DisqualifyLead(int leadId)
        {
            var lead = await _context.Leads.FindAsync(leadId);
            if (lead == null)
                return NotFound("Lead not found.");

            // Optionally prevent disqualifying converted leads
            if (lead.Status == LeadStatus.Converted)
                return BadRequest("Converted leads cannot be disqualified.");

            lead.Status = LeadStatus.Disqualified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Lead disqualified.", status = lead.Status.ToString() });
        }

        [HttpPost("create-from-lead")]
        public async Task<IActionResult> CreateOpportunityFromLead([FromBody] OpportunityCreateModel model)
        {
            var lead = await _context.Leads.FirstOrDefaultAsync(l => l.LeadId == model.LeadId);
            if (lead == null)
                return NotFound("Lead not found.");

            if (lead.Status != LeadStatus.Qualified)
                return BadRequest("Lead must be qualified before creating an opportunity.");

            // Create new deal/opportunity
            var deal = new Deal
            {
                LeadId = lead.LeadId,
                AssignedToSalesRep = lead.AssignedToSalesRep,
                DealName = model.Title,
                CreatedAt = DateTime.UtcNow,
                Status = DealStatus.Open 
            };

            _context.Deals.Add(deal);

            // Update the lead’s status to Converted
            lead.Status = LeadStatus.Converted;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Opportunity created successfully.", DealId = deal.DealId });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(int id)
        {
            var deleted = await _leadsService.DeleteLeadAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/communications")]
        public async Task<IActionResult> GetLeadCommunications(int id)
        {
            var logs = await _logService.GetByLeadIdAsync(id);
            return Ok(logs);
        }
    }
}
