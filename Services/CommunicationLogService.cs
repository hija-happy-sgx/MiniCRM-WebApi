using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Services
{
    public class CommunicationLogService
    {
        private readonly CrmDbContext _context;

        public CommunicationLogService(CrmDbContext context)
        {
            _context = context;
        }

        // Get all communications for a lead
        public async Task<IEnumerable<CommunicationLog>> GetByLeadIdAsync(int leadId)
        {
            return await _context.CommunicationLogs
                .Where(c => c.LeadId == leadId)
                .ToListAsync();
        }

        // Get all communications for a deal
        public async Task<IEnumerable<CommunicationLog>> GetByDealIdAsync(int dealId)
        {
            return await _context.CommunicationLogs
                .Where(c => c.DealId == dealId)
                .ToListAsync();
        }

        // Add a new communication log
        public async Task<CommunicationLog> AddAsync(CommunicationLog log)
        {
            _context.CommunicationLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        // Optional: Update log
        public async Task<bool> UpdateAsync(CommunicationLog log)
        {
            var existing = await _context.CommunicationLogs.FindAsync(log.LogId);
            if (existing == null) return false;

            existing.LeadId = log.LeadId;
            existing.DealId = log.DealId;
            existing.SalesRepId = log.SalesRepId;
            existing.Notes = log.Notes;
            existing.LogDate = log.LogDate;

            await _context.SaveChangesAsync();
            return true;
        }

        // Optional: Delete log
        public async Task<bool> DeleteAsync(int logId)
        {
            var log = await _context.CommunicationLogs.FindAsync(logId);
            if (log == null) return false;

            _context.CommunicationLogs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
