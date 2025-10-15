using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        
            private readonly ManagerService _managerService;

            public ManagerController(ManagerService managerService)
            {
                _managerService = managerService;
            }

            // 1️⃣ List SalesRep Managers under this Manager
            [HttpGet("salesrepmanagers")]

            public async Task<IActionResult> GetSalesRepManagers([FromQuery] int managerId)
            {
            
                var result = await _managerService.GetSalesRepManagersAsync(managerId);
                return Ok(result);
            }

            // 2️⃣ Assign SalesRep Manager to Manager
            [HttpPost("salesrepmanager/assign")]
            public async Task<IActionResult> AssignSalesRepManager([FromQuery] int managerId, [FromQuery] int srmId)
            {
                var success = await _managerService.AssignSalesRepManagerAsync(managerId, srmId);
                return Ok(new { success });
            }

            // 3️⃣ List all SalesReps under their SalesRep Managers
            [HttpGet("salesreps")]
            public async Task<IActionResult> GetSalesReps([FromQuery] int managerId)
            {
                var result = await _managerService.GetSalesRepsAsync(managerId);
                return Ok(result);
            }

            // 4️⃣ Reassign SalesRep to another SRM
            [HttpPatch("salesrep/reassign")]
            public async Task<IActionResult> ReassignSalesRep([FromQuery] int salesRepId, [FromQuery] int newSrmId)
            {
                var success = await _managerService.ReassignSalesRepAsync(salesRepId, newSrmId);
                return Ok(new { success });
            }

            // 5️⃣ List all leads under their SRMs
            [HttpGet("leads")]
            public async Task<IActionResult> GetLeads([FromQuery] int managerId)
            {
                var result = await _managerService.GetLeadsAsync(managerId);
                return Ok(result);
            }

            // 6️⃣ List all deals under their SRMs
            [HttpGet("deals")]
            public async Task<IActionResult> GetDeals([FromQuery] int managerId)
            {
                var result = await _managerService.GetDealsAsync(managerId);
                return Ok(result);
            }

            // 7️⃣ Approve deal stage changes or discounts
            [HttpPatch("deal/{id}/approve")]
            public async Task<IActionResult> ApproveDeal(int id, [FromQuery] int managerId)
            {
                var success = await _managerService.ApproveDealAsync(managerId, id);
                return Ok(new { success });
            }
        }
}
