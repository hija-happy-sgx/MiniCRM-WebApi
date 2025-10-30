using CRMWepApi.Data;
using CRMWepApi.DTOs;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRMWepApi.Services
{
    public class LeadsService
    {
        private readonly CrmDbContext _context;

        public LeadsService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadDto>> GetAllLeadsAsync()
        {
            return await _context.Leads
                .Select(l => new LeadDto
                {
                    LeadId = l.LeadId,
                    Name = l.Name,
                    Email = l.Email,
                    Phone = l.Phone,
                    Company = l.Company,
                    Source = l.Source,
                    Status = l.Status,
                    AssignedToSalesRep = l.AssignedToSalesRep,
                    SalesRepName = l.SalesRep != null ? l.SalesRep.Name : null,
                    AssignedToSrm = l.AssignedToSrm,
                    SrmName = l.AssignedSrm != null ? l.AssignedSrm.Name : null,
                    CreatedByManager = l.CreatedByManager,
                    ManagerName = l.Manager != null ? l.Manager.Name : null,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }


        public async Task<LeadDto?> GetLeadByIdAsync(int id)
        {
            return await _context.Leads
                .Where(l => l.LeadId == id)
                .Select(l => new LeadDto
                {
                    LeadId = l.LeadId,
                    Name = l.Name,
                    Email = l.Email,
                    Phone = l.Phone,
                    Company = l.Company,
                    Source = l.Source,
                    Status = l.Status,
                    AssignedToSalesRep = l.AssignedToSalesRep,
                    SalesRepName = l.SalesRep != null ? l.SalesRep.Name : null,
                    AssignedToSrm = l.AssignedToSrm,
                    SrmName = l.AssignedSrm != null ? l.AssignedSrm.Name : null,
                    CreatedByManager = l.CreatedByManager,
                    ManagerName = l.Manager != null ? l.Manager.Name : null,
                    CreatedAt = l.CreatedAt
                })
                .FirstOrDefaultAsync();
        }


        //public async Task<IEnumerable<Lead>> GetLeadsBySalesRepAsync(int salesRepId)
        //{
        //    return await _context.Leads
        //        .Where(l => l.AssignedToSalesRep == salesRepId)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<LeadDto>> GetLeadsBySalesRepAsync(int salesRepId)
        {
            return await _context.Leads
                .Where(l => l.AssignedToSalesRep == salesRepId)
                .Select(l => new LeadDto
                {
                    LeadId = l.LeadId,
                    Name = l.Name,
                    Email = l.Email,
                    Phone = l.Phone,
                    Company = l.Company,
                    Source = l.Source,
                    Status = l.Status,
                    AssignedToSalesRep = l.AssignedToSalesRep,
                    SalesRepName = l.SalesRep != null ? l.SalesRep.Name : null,
                    AssignedToSrm = l.AssignedToSrm,
                    SrmName = l.AssignedSrm != null ? l.AssignedSrm.Name : null,
                    CreatedByManager = l.CreatedByManager,
                    ManagerName = l.Manager != null ? l.Manager.Name : null,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }




        //public async Task<Lead> CreateLeadAsync(Lead lead)
        //{
        //    _context.Leads.Add(lead);
        //    await _context.SaveChangesAsync();
        //    return lead;
        //}

        public async Task<Lead> CreateLeadAsync(Lead lead)
        {
            // Basic validation or defaults
            if (string.IsNullOrWhiteSpace(lead.Name))
                throw new ArgumentException("Lead Name is required.");

            if (lead.AssignedToSalesRep.HasValue)
            {
                var exists = await _context.SalesReps.AnyAsync(r => r.SalesRepId == lead.AssignedToSalesRep.Value);
                if (!exists)
                    throw new ArgumentException("Assigned SalesRep does not exist.");
            }

            // Auto-set timestamps
            lead.CreatedAt = DateTime.UtcNow;
            lead.UpdatedAt = DateTime.UtcNow;

            //// Optional: Default status
            //if (string.IsNullOrEmpty(lead.Status.ToString()))
            //    lead.Status() = "New";

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
            existing.Company = lead.Company;
            existing.Status = lead.Status;
            existing.AssignedToSalesRep = lead.AssignedToSalesRep;
            existing.UpdatedAt = DateTime.UtcNow;

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
