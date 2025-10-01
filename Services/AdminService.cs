using CRMWepApi.Data;
using CRMWepApi.DTOs;
using CRMWepApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CRMWepApi.Services
{
    public class AdminService
    {
        private readonly CrmDbContext _context;

        public AdminService(CrmDbContext context)
        {
            _context = context;
        }

        //to list all users (managers, srms, salesreps)
        public async Task<object> GetAllUsersAsync()
        {
            var managers = await _context.Managers
                .Select(m => new { m.ManagerId, m.Name, m.Email, Role = "Manager" }).ToListAsync();

            var srms = await _context.SalesRepManagers
                .Select(s => new { s.SrmId, s.Name, s.Email, Role = "SalesRepManager" }).ToListAsync();

            var srs = await _context.SalesReps
                .Select(s => new { s.SalesRepId, s.Name, s.Email, Role = "SalesRep" }).ToListAsync();

            return new { Managers = managers, SalesRepManagers = srms, SalesReps = srs };
        }



        // create manager

        public async Task<object> CreateManagerAsync(RegisterUserDto Dto)
        {
            if (await _context.Managers.AnyAsync(m => m.Email == Dto.Email))
                throw new Exception("Manager wth this email already exists");

            var manager = new Manager
            {
                Name = Dto.Name,
                Email = Dto.Email,
                PasswordHash = HashPassword(Dto.Password),
                CreatedAt = DateTime.UtcNow,
                
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        public async Task<SalesRepManager> CreateSalesRepManagerAsync(RegisterUserDto dto, int managerId) 
        {
            if (await _context.SalesRepManagers.AnyAsync(m => m.Email == dto.Email))
                throw new Exception("Sales Rep Manager with this email is already existing");

            var srm = new SalesRepManager
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                ManagerId = managerId
            };

            _context.SalesRepManagers.Add(srm);
            await _context.SaveChangesAsync();
            return srm;
        }

        public async Task<SalesRep> CreateSalesRepAsync(RegisterUserDto dto, int srmId)
        {
            if(await _context.SalesReps.AnyAsync(m=> m.Email == dto.Email))
                throw new Exception("sales rep with this email alreafy exists");

            var sr = new SalesRep
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                SrmId = srmId
            };

             _context.SalesReps.Add(sr);
            await _context.SaveChangesAsync();
            return sr; 

        }

        // to update the role of a user (manager, srm, salesrep)

        public async Task UpdateUserAsync(int id, RegisterUserDto dto, string role)
        {
            switch (role.ToLower())
            {
                case "manager":
                    var manager = await _context.Managers.FindAsync(id);
                    if (manager == null) throw new Exception("Manager not found");
                    manager.Name = dto.Name;
                    manager.Email = dto.Email;
                    if (!string.IsNullOrEmpty(dto.Password))
                        manager.PasswordHash = HashPassword(dto.Password);
                    break;
                case "salesrepmanager":
                    var srm = await _context.SalesRepManagers.FindAsync(id);
                    if (srm == null) throw new Exception("SalesRepManager not found");
                    srm.Name = dto.Name;
                    srm.Email = dto.Email;
                    if (!string.IsNullOrEmpty(dto.Password))
                        srm.PasswordHash = HashPassword(dto.Password);
                    break;
                case "salesrep":
                    var sr = await _context.SalesReps.FindAsync(id);
                    if (sr == null) throw new Exception("SalesRep not found");
                    sr.Name = dto.Name;
                    sr.Email = dto.Email;
                    if (!string.IsNullOrEmpty(dto.Password))
                        sr.PasswordHash = HashPassword(dto.Password);
                    break;
            }

            await _context.SaveChangesAsync();
        }

        // Delete user
        public async Task DeleteUserAsync(int id, string role)
        {
            switch (role.ToLower())
            {
                case "manager":
                    var manager = await _context.Managers.FindAsync(id);
                    if (manager == null) throw new Exception("Manager not found");
                    _context.Managers.Remove(manager);
                    break;
                case "salesrepmanager":
                    var srm = await _context.SalesRepManagers.FindAsync(id);
                    if (srm == null) throw new Exception("SalesRepManager not found");
                    _context.SalesRepManagers.Remove(srm);
                    break;
                case "salesrep":
                    var sr = await _context.SalesReps.FindAsync(id);
                    if (sr == null) throw new Exception("SalesRep not found");
                    _context.SalesReps.Remove(sr);
                    break;
            }
            await _context.SaveChangesAsync();
        }

        // List pipeline stages
        public async Task<List<PipelineStage>> GetPipelineStagesAsync()
        {
            return await _context.PipelineStages.OrderBy(p => p.StageOrder).ToListAsync();
        }

        // Add pipeline stage
        public async Task<PipelineStage> AddPipelineStageAsync(string name, int order)
        {
            var stage = new PipelineStage
            {
                StageName = name,
                StageOrder = order
            };
            _context.PipelineStages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
