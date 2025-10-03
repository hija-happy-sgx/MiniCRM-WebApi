using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;
//using MiniCRM.Core.Models;
//using MiniCRM.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWepApi.Services
{
    public class SalesRepManagerService
    {
        private readonly CrmDbContext _context;

        public SalesRepManagerService(CrmDbContext context)
        {
            _context = context;
        }

        // Dashboard summary for a manager
        public object GetDashboard(int managerId)
        {
            var salesReps = _context.SalesReps
                .Where(r => r.SrmId == managerId)
                .Select(r => new {
                    SalesRepId = r.SalesRepId,
                    SalesRepName = r.Name,
                    LeadsCount = _context.Leads.Count(l => l.AssignedToSalesRep == r.SalesRepId),
                    DealsCount = _context.Deals.Count(d => d.AssignedToSalesRep == r.SalesRepId)
                })
                .ToList();

            return new
            {
                TeamSize = salesReps.Count,
                TotalLeads = salesReps.Sum(r => r.LeadsCount),
                TotalDeals = salesReps.Sum(r => r.DealsCount),
                SalesReps = salesReps
            };
        }

        // List SalesReps under manager
        public IEnumerable<SalesRep> GetSalesReps(int managerId)
        {
            return _context.SalesReps
                .Where(r => r.SrmId == managerId)
                .ToList();
        }

        // Assign lead to SalesRep
        public Lead AssignLead(int leadId, int salesRepId, int managerId)
        {
            // only allow if manager owns the SalesRep
            var salesRep = _context.SalesReps.FirstOrDefault(r => r.SalesRepId == salesRepId && r.SrmId == managerId);
            if (salesRep == null) return null;

            var lead = _context.Leads.FirstOrDefault(l => l.LeadId == leadId);
            if (lead == null) return null;

            lead.AssignedToSalesRep = salesRepId;
            _context.SaveChanges();

            return lead;
        }

        // Leads under manager’s team
        public IEnumerable<Lead> GetTeamLeads(int managerId)
        {
            var repIds = _context.SalesReps
                .Where(r => r.SrmId == managerId)
                .Select(r => r.SalesRepId)
                .ToList();

            return _context.Leads
                .Where(l => l.AssignedToSalesRep != null && repIds.Contains(l.AssignedToSalesRep.Value))
                .ToList();
        }

        // Deals under manager’s team
        public IEnumerable<Deal> GetTeamDeals(int managerId)
        {
            var repIds = _context.SalesReps
                .Where(r => r.SrmId == managerId)
                .Select(r => r.SalesRepId)
                .ToList();

            return _context.Deals
                .Where(d => d.AssignedToSalesRep != null && repIds.Contains(d.AssignedToSalesRep.Value))
                .Include(d => d.Stage)
                .ToList();
        }

        // Communication logs for all leads/deals in team
        public IEnumerable<CommunicationLog> GetTeamCommunicationLogs(int managerId)
        {
            var repIds = _context.SalesReps
                .Where(r => r.SrmId == managerId)
                .Select(r => r.SalesRepId)
                .ToList();

            return _context.CommunicationLogs
       .Where(c =>
           (c.LeadId != null && _context.Leads.Any(l => l.LeadId == c.LeadId && l.AssignedToSalesRep != null && repIds.Contains(l.AssignedToSalesRep.Value))) ||
           (c.DealId != null && _context.Deals.Any(d => d.DealId == c.DealId && d.AssignedToSalesRep != null && repIds.Contains(d.AssignedToSalesRep.Value)))
       )
       .ToList();
        }

        // Pipeline stages
        public IEnumerable<PipelineStage> GetPipelineStages()
        {
            return _context.PipelineStages
                .OrderBy(p => p.StageOrder)
                .ToList();
        }
    }

}
