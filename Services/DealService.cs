using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Services
{
    public class DealsService
    {
        private readonly CrmDbContext _context;

        public DealsService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deal>> GetAllDealsAsync()
        {
            return await _context.Deals.ToListAsync();
        }

        public async Task<Deal?> GetDealByIdAsync(int id)
        {
            return await _context.Deals.FindAsync(id);
        }

        public async Task<Deal> CreateDealAsync(Deal deal)
        {
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            return deal;
        }

        public async Task<bool> UpdateDealAsync(Deal deal)
        {
            var existing = await _context.Deals.FindAsync(deal.DealId);
            if (existing == null) return false;

            existing.DealName = deal.DealName;
            existing.LeadId = deal.LeadId;
            existing.Value = deal.Value;
            existing.Status = deal.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDealAsync(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null) return false;

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
