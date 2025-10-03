using CRMWepApi.Data;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMWepApi.Services
{
    public class PipelineService
    {
        private readonly CrmDbContext _context;

        public PipelineService(CrmDbContext context)
        {
            _context = context;
        }

        // Get pipelines for a user based on role
        public async Task<IEnumerable<PipelineStage>> GetPipelinesForUserAsync(string role, int userId)
        {
            switch (role)
            {
                case "Admin":
                    return await _context.PipelineStages
                        .OrderBy(p => p.StageOrder)
                        .ToListAsync();

                case "Manager":
                    // Manager sees pipelines for their SalesRepManagers and SalesReps
                    var repManagerIds = _context.SalesRepManagers
                        .Where(srm => srm.ManagerId == userId)
                        .Select(srm => srm.SrmId)
                        .ToList();

                    var salesRepIds = _context.SalesReps
                        .Where(sr => repManagerIds.Contains(sr.SrmId))
                        .Select(sr => sr.SalesRepId)
                        .ToList();

                    return await _context.PipelineStages
                        .Where(p => salesRepIds.Contains(p.AssignedToSalesRepId??0)
                                 || repManagerIds.Contains(p.AssignedToSrmId ?? 0))
                        .OrderBy(p => p.StageOrder)
                        .ToListAsync();

                case "SalesRepManager":
                    var teamSalesRepIds = _context.SalesReps
                        .Where(sr => sr.SrmId == userId)
                        .Select(sr => sr.SalesRepId)
                        .ToList();

                    return await _context.PipelineStages
                        .Where(p => p.AssignedToSrmId == userId || teamSalesRepIds.Contains(p.AssignedToSalesRepId?? 0))
                        .OrderBy(p => p.StageOrder)
                        .ToListAsync();

                case "SalesRep":
                    return await _context.PipelineStages
                        .Where(p => p.AssignedToSalesRepId == userId)
                        .OrderBy(p => p.StageOrder)
                        .ToListAsync();

                default:
                    return new List<PipelineStage>();
            }
        }

        // CRUD methods for Admin (same as before)
        public async Task<PipelineStage> CreateStageAsync(PipelineStage stage)
        {
            _context.PipelineStages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<PipelineStage?> UpdateStageAsync(int id, PipelineStage updatedStage)
        {
            var stage = await _context.PipelineStages.FindAsync(id);
            if (stage == null) return null;

            stage.StageName = updatedStage.StageName;
            stage.StageOrder = updatedStage.StageOrder;

            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<bool> DeleteStageAsync(int id)
        {
            var stage = await _context.PipelineStages.FindAsync(id);
            if (stage == null) return false;

            _context.PipelineStages.Remove(stage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
