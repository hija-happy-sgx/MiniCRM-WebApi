using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Services
{
    public class ManagerService
    {
        private readonly CrmDbContext _context;

        public ManagerService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesRepManager>> GetSalesRepManagersAsync(int managerId)
        {
            return await _context.SalesRepManagers
                .Where(srm => srm.ManagerId == managerId)
                .ToListAsync();
        }


        public async Task<bool> AssignSalesRepManagerAsync(int managerId, int srmId)
        {
            var srm = await _context.SalesRepManagers.FindAsync(srmId);
            if (srm == null) return false;

            srm.ManagerId = managerId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SalesRep>> GetSalesRepsAsync(int managerId)
        {
            return await _context.SalesReps
                .Where(rep => rep.SalesRepManager.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<bool> ReassignSalesRepAsync(int salesRepId, int newSrmId)
        {
            var rep = await _context.SalesReps.FindAsync(salesRepId);
            if (rep == null) return false;

            rep.SrmId = newSrmId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync(int managerId)
        {
            return await _context.Leads
       .Where(l => l.SalesRep != null
                && l.SalesRep.SalesRepManager != null
                && l.SalesRep.SalesRepManager.ManagerId == managerId)
       .ToListAsync();
        }

        public async Task<IEnumerable<Deal>> GetDealsAsync(int managerId)
        {
            return await _context.Deals
                .Where(d => d.AssignedSrm.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<bool> ApproveDealAsync(int managerId, int dealId)
        {
            var deal = await _context.Deals.FindAsync(dealId);
            if (deal == null) return false;

            deal.IsApprovedByManager = true;  // add this field in Deal model if missing
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
