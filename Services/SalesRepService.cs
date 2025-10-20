using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Services
{
    public class SalesRepService
    {
        private readonly CrmDbContext _context;

        public SalesRepService(CrmDbContext context)
        {
            _context = context;
        }

        // Dashboard: Leads, Deals, Tasks/Follow-ups
        public async Task<object> GetDashboardAsync(int salesRepId)
        {
            var leads = await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId)
                .ToListAsync();

            var deals = await _context.Deals
                .Where(d => d.AssignedToSalesRep == salesRepId)
                .ToListAsync();

            var communications = await _context.CommunicationLogs
                .Where(c => c.SalesRepId == salesRepId)
                .ToListAsync();

            // Compute metrics
            var totalLeads = leads.Count;
            var activeDeals = deals.Count(d => d.Status == Enums.DealStatus.Open);
            var wonDeals = deals.Count(d => d.Status == Enums.DealStatus.Won);
            var totalRevenue = deals.Sum(d => d.Value);

            // Example logic for rates
            var leadsConversionRate = totalLeads > 0 ? (double)wonDeals / totalLeads * 100 : 0;
            var dealsClosingRate = deals.Count > 0 ? (double)wonDeals / deals.Count * 100 : 0;

            // Take last 5 communications as recent activity
            var recentActivity = communications.Take(5)
                .Select(c => new {
                    type = c.Type,
                    description = c.Notes,
                    date = c.LogDate.ToString("yyyy-MM-dd HH:mm") // format as needed
                }).ToList();

            // Return object matching your Angular template
            return new
            {
                totalLeads,
                activeDeals,
                wonDeals,
                totalRevenue,
                leadsConversionRate,
                dealsClosingRate,
                recentActivity
            };
        }

        // Leads
        public async Task<IEnumerable<Lead>> GetLeadsAsync(int salesRepId)
        {
            return await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId)
                .ToListAsync();
        }

        public async Task<Lead> CreateLeadAsync(Lead lead, int salesRepId)
        {
            lead.AssignedToSalesRep = salesRepId;
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<Lead> GetLeadByIdAsync(int leadId, int salesRepId)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(l => l.LeadId == leadId && l.AssignedToSalesRep == salesRepId);
        }

        public async Task<bool> UpdateLeadAsync(Lead updatedLead, int salesRepId)
        {
            var lead = await _context.Leads
                .FirstOrDefaultAsync(l => l.LeadId == updatedLead.LeadId && l.AssignedToSalesRep == salesRepId);

            if (lead == null) return false;

            lead.Name = updatedLead.Name;
            lead.Company = updatedLead.Company;
            lead.Email = updatedLead.Email;
            lead.Phone = updatedLead.Phone;
            lead.Status = updatedLead.Status;
            lead.UpdatedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Deals
        public async Task<IEnumerable<Deal>> GetDealsAsync(int salesRepId)
        {
            return await _context.Deals
                .Where(d => d.AssignedToSalesRep == salesRepId)
                .ToListAsync();
        }

        public async Task<Deal> GetDealByIdAsync(int dealId, int salesRepId)
        {
            return await _context.Deals
                .FirstOrDefaultAsync(d => d.DealId == dealId && d.AssignedToSalesRep == salesRepId);
        }

        public async Task<bool> UpdateDealAsync(Deal updatedDeal, int salesRepId)
        {
            var deal = await _context.Deals
                .FirstOrDefaultAsync(d => d.DealId == updatedDeal.DealId && d.AssignedToSalesRep == salesRepId);

            if (deal == null) return false;

            deal.StageId = updatedDeal.StageId;
            deal.Status = updatedDeal.Status;
            deal.Value = updatedDeal.Value;
            deal.UpdatedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Communication Logs
        public async Task<CommunicationLog> AddCommunicationAsync(CommunicationLog log)
        {
            log.LogDate = System.DateTime.UtcNow;
            _context.CommunicationLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<IEnumerable<CommunicationLog>> GetCommunicationsAsync(int salesRepId)
        {
            return await _context.CommunicationLogs
                .Where(c => c.SalesRepId == salesRepId)
                .ToListAsync();
        }
    }
}
