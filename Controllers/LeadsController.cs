using CRMWepApi.DTOs;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeadsController : ControllerBase
    {
        private readonly LeadsService _leadsService;
        private readonly CommunicationLogService _logService;

        public LeadsController(LeadsService leadsService)
        {
            _leadsService = leadsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeads()
        {
            var leads = await _leadsService.GetAllLeadsAsync();
            return Ok(leads);
        }

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
