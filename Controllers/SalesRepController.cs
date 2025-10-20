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

        [HttpPatch("lead/{id}")]
        public async Task<IActionResult> UpdateLead(int id, [FromBody] Models.Lead lead, [FromQuery] int salesRepId)
        {
            lead.LeadId = id;
            var success = await _service.UpdateLeadAsync(lead, salesRepId);
            return Ok(new { success });
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

        [HttpPost("communication")]
        public async Task<IActionResult> AddCommunication([FromBody] Models.CommunicationLog log)
        {
            var result = await _service.AddCommunicationAsync(log);
            return Ok(result);
        }

        [HttpGet("communication")]
        public async Task<IActionResult> GetCommunications([FromQuery] int salesRepId)
        {
            var result = await _service.GetCommunicationsAsync(salesRepId);
            return Ok(result);
        }

    }
}
