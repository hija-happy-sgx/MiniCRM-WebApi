using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SalesReps")]
    public class SalesRepController : ControllerBase
    {
        private readonly SalesRepService _service;

        public SalesRepController(SalesRepService service)
        {
            _service = service;
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
        public async Task<IActionResult> CreateLead([FromBody] Models.Lead lead, [FromQuery] int salesRepId)
        {
            var result = await _service.CreateLeadAsync(lead, salesRepId);
            return Ok(result);
        }

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
