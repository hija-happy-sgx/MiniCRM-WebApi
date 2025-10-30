using CRMWepApi.Data;
using CRMWepApi.Enums;
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

        public async Task<IEnumerable<Deal>> GetDealsBySalesRepAsync(int salesRepId)
        {
            return await _context.Deals
                .Where(d => d.AssignedToSalesRep == salesRepId)
                .ToListAsync();
        }


        public async Task<Deal> CreateDealAsync(Deal deal)
        {
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            return deal;
        }

        public async Task<Deal> CreateDealAndConvertLeadAsync(Deal deal)
        {
            //  database transaction to ensure atomicity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // CREATE THE DEAL
                    _context.Deals.Add(deal);
                    await _context.SaveChangesAsync();

                    // FIND AND UPDATE THE LEAD STATUS
                    var lead = await _context.Leads.FindAsync(deal.LeadId);

                    if (lead == null)
                    {
                       
                        throw new KeyNotFoundException($"Lead with ID {deal.LeadId} not found during deal conversion.");
                    }

                    
                    lead.Status = LeadStatus.Converted; 
                    lead.UpdatedAt = DateTime.UtcNow;

                    _context.Leads.Update(lead);
                    await _context.SaveChangesAsync();

                    // COMMIT TRANSACTION
                    await transaction.CommitAsync();
                    return deal;
                }
                catch (Exception ex)
                {
                    
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Transaction rolled back: {ex.Message}");
                    throw; 
                }
            }
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
