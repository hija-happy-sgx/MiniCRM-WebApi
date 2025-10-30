using CRMWepApi.Data;
using CRMWepApi.DTOs;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SalesRep")]
    public class SalesRepController : ControllerBase
    {
        private readonly SalesRepService _service;
        private readonly LeadsService _leadService;
        private readonly CrmDbContext _context;



        public SalesRepController(SalesRepService service, LeadsService leadService, CrmDbContext context  )
        {
            _service = service;
            _leadService = leadService;
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard([FromQuery] int salesRepId)
        {
            var result = await _service.GetDashboardAsync(salesRepId);
            return Ok(result);
        }



        [HttpGet("leads")]
        public async Task<IActionResult> GetLeads([FromQuery] int salesRepId)
        {
            var result = await _service.GetLeadsAsync(salesRepId);
            return Ok(result);
        }

        [HttpPost("leads")]
        public async Task<IActionResult> CreateLead([FromBody] LeadCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var salesRepId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"
            );

            // Include SRM and Manager
            var salesRep = await _context.SalesReps
                .Include(s => s.SalesRepManager)          // SRM
                    .ThenInclude(srm => srm.Manager)      // Manager
                .FirstOrDefaultAsync(s => s.SalesRepId == salesRepId);

            if (salesRep == null)
                return BadRequest("Assigned SalesRep does not exist.");

            // Extract SRM and Manager IDs dynamically
            var srmId = salesRep.SalesRepManager?.SrmId; // SRM ID
            var managerId = salesRep.SalesRepManager?.Manager?.ManagerId; // Manager ID

            var lead = new Lead
            {
                Name = dto.Name,
                Company = dto.Company,
                Email = dto.Email,
                Phone = dto.Phone,
                Source = dto.Source,
                Status = dto.Status ?? LeadStatus.New,
                AssignedToSalesRep = salesRepId,
                AssignedToSrm = srmId,
                CreatedByManager = managerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _leadService.CreateLeadAsync(lead);

            var result = new LeadDto
            {
                LeadId = created.LeadId,
                Name = created.Name,
                Email = created.Email,
                Phone = created.Phone,
                Company = created.Company,
                Source = created.Source,
                Status = created.Status,
                AssignedToSalesRep = created.AssignedToSalesRep,
                SalesRepName = created.SalesRep?.Name,
                AssignedToSrm = created.AssignedToSrm,
                SrmName = created.AssignedSrm?.Name,
                CreatedByManager = created.CreatedByManager,
                ManagerName = created.Manager?.Name,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction("GetLead", "Leads", new { id = created.LeadId }, result);
        }

        //[HttpPost("leads")]
        //public IActionResult CreateLeadDebug([FromBody] object body)
        //{
        //    var json = System.Text.Json.JsonSerializer.Serialize(body);
        //    Console.WriteLine("RAW JSON RECEIVED: " + json);
        //    return Ok();
        //}


        //[HttpPost("leads")]
        //public async Task<IActionResult> CreateLead([FromBody] Models.Lead lead)
        //{
        //    // Get the logged-in salesRepId from JWT
        //    var salesRepId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        //    lead.AssignedToSalesRep = salesRepId;

        //    var created = await _leadService.CreateLeadAsync(lead);
        //    return CreatedAtAction("GetLead", "Leads", new { id = created.LeadId }, created);
        //}


        //[HttpPost("leads")]
        //public async Task<IActionResult> CreateLead([FromBody] Models.Lead lead)
        //{
        //    // Optionally assign the sales rep here
        //    // lead.AssignedToSalesRep = salesRepIdFromTokenOrQuery;

        //    var created = await _leadService.CreateLeadAsync(lead);
        //    return CreatedAtAction("GetLead", "Leads", new { id = created.LeadId }, created);
        //}


        //[HttpPost("leads")]
        //public async Task<IActionResult> CreateLead([FromBody] Models.Lead lead, [FromQuery] int salesRepId)
        //{
        //    var result = await _service.CreateLeadAsync(lead, salesRepId);
        //    return Ok(result);
        //}

        [HttpGet("lead/{id}")]
        public async Task<IActionResult> GetLead(int id, [FromQuery] int salesRepId)
        {
            var result = await _service.GetLeadByIdAsync(id, salesRepId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        //[HttpGet("unconverted-leads")] 
        //public async Task<IActionResult> GetUnconvertedLeads([FromQuery] int salesRepId)
        //{


        //    var unconvertedStatuses = new LeadStatus[] 
        //    { 
        //        LeadStatus.New, 
        //        LeadStatus.Qualified, 
        //        LeadStatus.Contacted 
        //    };

        //    var leads = await _context.Leads
        //        .Where(l => l.AssignedToSalesRep == salesRepId && unconvertedStatuses.Contains(l.Status))
        //        .Select(l => new 
        //        {
        //            LeadId = l.LeadId,
        //            Name = l.Name,
        //            Company = l.Company ?? "N/A"
        //        })
        //        .ToListAsync();

        //    return Ok(leads);
        //}

        [HttpGet("unconverted-leads")]
        public async Task<IActionResult> GetUnconvertedLeads(int salesRepId)
        {
            var leads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Qualified)
                .Select(l => new {
                    l.LeadId,
                    Name = l.Name,
                    Company = l.Company
                })
                .ToListAsync();

            return Ok(leads);
        }


        [HttpPatch("lead/{id}")]
        public async Task<IActionResult> UpdateLead(int id, [FromBody] Models.Lead lead, [FromQuery] int salesRepId)
        {
            lead.LeadId = id;
            var success = await _service.UpdateLeadAsync(lead, salesRepId);
            return Ok(new { success });
        }


        [HttpGet("qualified-leads")]
        public async Task<IActionResult> GetQualifiedLeads(int salesRepId)
        {
            var qualifiedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Qualified)
                .Select(l => new
                {
                    l.LeadId,
                    l.Name
                })
                .ToListAsync();

            return Ok(qualifiedLeads);
        }

        [HttpGet("disqualified-leads")]
        public async Task<IActionResult> GetDisqualifiedLeads(int salesRepId)
        {
            var disqualifiedLeads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId && l.Status == LeadStatus.Disqualified)
                .Select(l => new
                {
                    l.LeadId,
                    l.Name
                })
                .ToListAsync();

            return Ok(disqualifiedLeads);
        }

        [HttpGet("deals")]
        public async Task<IActionResult> GetDeals([FromQuery] int salesRepId)
        {
            var result = await _service.GetDealsAsync(salesRepId);
            return Ok(result);
        }

        [HttpGet("deal/{id}")]
        public async Task<IActionResult> GetDeal(int id, [FromQuery] int salesRepId)
        {
            var result = await _service.GetDealByIdAsync(id, salesRepId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPatch("deal/{id}")]
        public async Task<IActionResult> UpdateDeal(int id, [FromBody]     Models.Deal deal, [FromQuery] int salesRepId)
        {
            deal.DealId = id;
            var success = await _service.UpdateDealAsync(deal, salesRepId);
            return Ok(new { success });
        }

        [HttpPost("deals")]
        public async Task<IActionResult> CreateDealAndConvertLead([FromBody] Models.Deal deal, [FromQuery] int salesRepId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //vvalidate mandatory fields
            if (deal.LeadId <= 0)
                return BadRequest("A LeadId is required to create a deal from a lead.");

            deal.AssignedToSalesRep = salesRepId;
            deal.CreatedAt = DateTime.UtcNow;

            // start a database transaction to ensure both operations succeed or fail together
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // CREATE THE DEAL

                    _context.Deals.Add(deal);
                    await _context.SaveChangesAsync();

                    // UPDATE THE LEAD STATUS
                    var lead = await _context.Leads.FindAsync(deal.LeadId);

                    if (lead == null)
                    {
                        //  trigger the catch block and rollback the deal creation
                        throw new Exception($"Lead with ID {deal.LeadId} not found.");
                    }

                    // Update the lead status to 'Converted' or a similar dedicated status

                    lead.Status = LeadStatus.Converted;
                    lead.UpdatedAt = DateTime.UtcNow;

                    _context.Leads.Update(lead);
                    await _context.SaveChangesAsync();

                    //  COMMIT TRANSACTION
                    await transaction.CommitAsync();

                    return CreatedAtAction("GetDeal", new { id = deal.DealId, salesRepId = salesRepId }, deal);
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    Console.WriteLine($"Transaction failed: {ex.Message}");
                    return StatusCode(500, new { message = "Failed to create deal and convert lead. Transaction rolled back." });
                }
            }

        }

            [HttpPost("communication")]
        public async Task<IActionResult> AddCommunication([FromBody] Models.CommunicationLog log)
        {
            var result = await _service.AddCommunicationAsync(log);
            return Ok(result);
        }

        [HttpGet("communication")]
        public async Task<IActionResult> GetCommunicationLogs(int salesRepId)
        {
            var logs = await _context.CommunicationLogs
                .Where(c => c.SalesRepId == salesRepId)
                .Include(c => c.Lead) // Include the lead navigation property
                .Select(c => new
                {
                    c.LogId,
                    c.Type,
                    c.Notes,
                    c.LogDate,
                    LeadId = c.LeadId,
                    LeadName = c.Lead.Name,
                    LeadStatus = c.Lead != null ? c.Lead.Status.ToString() : "Unknown"
                })
                .OrderByDescending(c => c.LogDate)
                .ToListAsync();

            return Ok(logs);
        }


    }
}
