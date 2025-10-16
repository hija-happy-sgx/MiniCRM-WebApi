using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "SalesRepManagers")]
   
        public class SalesRepManagerController : ControllerBase
        {
            private readonly SalesRepManagerService _service;

            public SalesRepManagerController(SalesRepManagerService service)
            {
                _service = service;
            }

            // Dashboard summary
            [HttpGet("{managerId}/dashboard")]
            public IActionResult GetDashboard(int managerId)
            {
                var result = _service.GetDashboard(managerId);
                return Ok(result);
            }

            // Get all sales reps under manager
            [HttpGet("{managerId}/salesreps")]
            public IActionResult GetSalesReps(int managerId)
            {
                var reps = _service.GetSalesReps(managerId);
                return Ok(reps);
            }

            // Assign a lead to a sales rep
            [HttpPost("{managerId}/assignlead/{leadId}/{salesRepId}")]
            public IActionResult AssignLead(int managerId, int leadId, int salesRepId)
            {
                var lead = _service.AssignLead(leadId, salesRepId, managerId);
                if (lead == null)
                    return NotFound("Lead not found or not accessible for this manager.");
                return Ok(lead);
            }

            // Leads under manager’s team
            [HttpGet("{managerId}/leads")]
            public IActionResult GetTeamLeads(int managerId)
            {
                var leads = _service.GetTeamLeads(managerId);
                return Ok(leads);
            }

            // Deals under manager’s team
            [HttpGet("{managerId}/deals")]
            public IActionResult GetTeamDeals(int managerId)
            {
                var deals = _service.GetTeamDeals(managerId);
                return Ok(deals);
            }

            // Communication logs for all team’s leads & deals
            [HttpGet("{managerId}/communicationlogs")]
            public IActionResult GetTeamCommunicationLogs(int managerId)
            {
                var logs = _service.GetTeamCommunicationLogs(managerId);
                return Ok(logs);
            }

            // Pipeline stages
            [HttpGet("pipelinestages")]
            public IActionResult GetPipelineStages()
            {
                var stages = _service.GetPipelineStages();
                return Ok(stages);
            }
        }
}
