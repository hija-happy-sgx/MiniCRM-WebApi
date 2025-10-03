using CRMWepApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CRMWepApi.Data
{
    public class CrmDbContext : DbContext
    {

        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<SalesRepManager> SalesRepManagers { get; set; }
        public DbSet<SalesRep> SalesReps { get; set; }
        public DbSet<PipelineStage> PipelineStages { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<CommunicationLog> CommunicationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Deal>()
                .Property(d => d.Status)
                .HasConversion<string>();

            modelBuilder
                .Entity<Lead>()
                .Property(l => l.Status)
                .HasConversion<string>();


            modelBuilder.Entity<Deal>()
                .HasOne(d => d.AssignedSrm)
                .WithMany()
                .HasForeignKey(d => d.AssignedToSrm)
                .OnDelete(DeleteBehavior.Restrict);

            // Deal → CreatedSrm relationship
            modelBuilder.Entity<Deal>()
                .HasOne(d => d.CreatedSrm)
                .WithMany()
                .HasForeignKey(d => d.CreatedBySrm)
                .OnDelete(DeleteBehavior.Restrict);

            // Similarly for Lead
            modelBuilder.Entity<Lead>()
                .HasOne(l => l.AssignedSrm)
                .WithMany()
                .HasForeignKey(l => l.AssignedToSrm)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lead>()
                .HasOne(l => l.CreatedSrm)
                .WithMany()
                .HasForeignKey(l => l.CreatedBySrm)
                .OnDelete(DeleteBehavior.Restrict);

            // Convert enums to string if needed
            modelBuilder.Entity<Deal>().Property(d => d.Status).HasConversion<string>();
            modelBuilder.Entity<Lead>().Property(l => l.Status).HasConversion<string>();

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Enum>()
                .HaveConversion<string>();
        }

    }
    }


