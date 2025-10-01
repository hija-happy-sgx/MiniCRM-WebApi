using CRMWepApi.Data;
using CRMWepApi.Models;

namespace CRMWepApi.Services
{
    public interface IAdminService
    {
        Task<UserResponseDto> CreateManagerAsync(CreateManagerDto dto);
        Task<UserResponseDto> CreateSrmAsync(CreateSrmDto dto);
        Task<UserResponseDto> CreateSalesRepAsync(CreateSalesRepDto dto);
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<PipelineStage> CreatePipelineStageAsync(CreatePipelineStageDto dto);
        Task<List<PipelineStage>> GetPipelineStagesAsync();
        Task<bool> DeleteUserAsync(int id);
    }

    public class AdminService : IAdminService
    {
        private readonly CrmDbContext _context;

        public AdminService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> CreateManagerAsync(CreateManagerDto dto)
        {
            var manager = new Manager
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = manager.ManagerId,
                Role = "Manager",
                Name = manager.Name,
                Email = manager.Email
            };
        }

        public async Task<UserResponseDto> CreateSrmAsync(CreateSrmDto dto)
        {
            var srm = new SalesRepManager
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                ManagerId = dto.ManagerId,
                CreatedAt = DateTime.UtcNow
            };

            _context.SalesRepManagers.Add(srm);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = srm.SrmId,
                Role = "SalesRepManager",
                Name = srm.Name,
                Email = srm.Email
            };
        }

        public async Task<UserResponseDto> CreateSalesRepAsync(CreateSalesRepDto dto)
        {
            var rep = new SalesRep
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                SrmId = dto.SrmId,
                CreatedAt = DateTime.UtcNow
            };

            _context.SalesReps.Add(rep);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = rep.SalesRepId,
                Role = "SalesRep",
                Name = rep.Name,
                Email = rep.Email
            };
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var managers = await _context.Managers
                .Select(m => new UserResponseDto
                {
                    Id = m.ManagerId,
                    Role = "Manager",
                    Name = m.Name,
                    Email = m.Email
                }).ToListAsync();

            var srms = await _context.SalesRepManagers
                .Select(s => new UserResponseDto
                {
                    Id = s.SrmId,
                    Role = "SalesRepManager",
                    Name = s.Name,
                    Email = s.Email
                }).ToListAsync();

            var reps = await _context.SalesReps
                .Select(r => new UserResponseDto
                {
                    Id = r.SalesRepId,
                    Role = "SalesRep",
                    Name = r.Name,
                    Email = r.Email
                }).ToListAsync();

            return managers.Concat(srms).Concat(reps).ToList();
        }

        public async Task<PipelineStage> CreatePipelineStageAsync(CreatePipelineStageDto dto)
        {
            var stage = new PipelineStage
            {
                StageName = dto.StageName,
                StageOrder = dto.StageOrder
            };

            _context.PipelineStages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<List<PipelineStage>> GetPipelineStagesAsync()
        {
            return await _context.PipelineStages.OrderBy(s => s.StageOrder).ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Managers.FindAsync(id)
                    ?? await _context.SalesRepManagers.FindAsync(id)
                    ?? await _context.SalesReps.FindAsync(id);

            if (user == null) return false;

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
