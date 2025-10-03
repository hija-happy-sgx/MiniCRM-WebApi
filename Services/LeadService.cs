using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Services
{
    public class LeadsService
    {
        private readonly CrmDbContext _context;

        public LeadsService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _context.Leads.ToListAsync();
        }

        public async Task<Lead?> GetLeadByIdAsync(int id)
        {
            return await _context.Leads.FindAsync(id);
        }

        public async Task<Lead> CreateLeadAsync(Lead lead)
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<bool> UpdateLeadAsync(Lead lead)
        {
            var existing = await _context.Leads.FindAsync(lead.LeadId);
            if (existing == null) return false;

            existing.Name = lead.Name;
            existing.Email = lead.Email;
            existing.Phone = lead.Phone;
            existing.AssignedToSalesRep = lead.AssignedToSalesRep;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLeadAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
