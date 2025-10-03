using CRMWepApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMWepApi.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }

    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SalesRepManager>? SalesRepManagers { get; set; }
        public ICollection<Lead>? LeadsCreated { get; set; }
        public ICollection<Deal>? DealsCreated { get; set; }
    }

    public class SalesRepManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SrmId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Manager))]
        public int ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public ICollection<SalesRep>? SalesReps { get; set; }
        public ICollection<Lead>? LeadsCreated { get; set; }
        public ICollection<Deal>? DealsCreated { get; set; }
    }

    public class SalesRep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesRepId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(SalesRepManager))]
        public int SrmId { get; set; }
        public SalesRepManager? SalesRepManager { get; set; }

        public ICollection<Lead>? Leads { get; set; }
        public ICollection<Deal>? Deals { get; set; }
        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }
    }

    public class PipelineStage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StageId { get; set; }

        public string StageName { get; set; } = null!;
        public int StageOrder { get; set; }

        // New field links for role-based visibility
        public int? AssignedToSrmId { get; set; }       // SalesRepManager
        public int? AssignedToSalesRepId { get; set; } // SalesRep
        public ICollection<Deal>? Deals { get; set; }
    }

    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeadId { get; set; }

        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.New;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(SalesRep))]
        public int? AssignedToSalesRep { get; set; }
        public SalesRep? SalesRep { get; set; }

        [ForeignKey(nameof(AssignedSrm))]
        public int? AssignedToSrm { get; set; }
        public SalesRepManager? AssignedSrm { get; set; }

        [ForeignKey(nameof(Manager))]
        public int? CreatedByManager { get; set; }
        public Manager? Manager { get; set; }

        [ForeignKey(nameof(CreatedSrm))]
        public int? CreatedBySrm { get; set; }
        public SalesRepManager? CreatedSrm { get; set; }

        public ICollection<Deal>? Deals { get; set; }
        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }
    }

    public class Deal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealId { get; set; }

        public string DealName { get; set; } = null!;
        public decimal Value { get; set; }
        public DealStatus Status { get; set; } = DealStatus.Open;

        public DateTime? ExpectedCloseDate { get; set; }
        public DateTime? ActualCloseDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Lead))]
        public int LeadId { get; set; }
        public Lead? Lead { get; set; }

        [ForeignKey(nameof(Stage))]
        public int StageId { get; set; }
        public PipelineStage? Stage { get; set; }

        [ForeignKey(nameof(SalesRep))]
        public int? AssignedToSalesRep { get; set; }
        public SalesRep? SalesRep { get; set; }

        [ForeignKey(nameof(AssignedSrm))]
        public int? AssignedToSrm { get; set; }
        public SalesRepManager? AssignedSrm { get; set; }

        [ForeignKey(nameof(Manager))]
        public int? CreatedByManager { get; set; }
        public Manager? Manager { get; set; }

        [ForeignKey(nameof(CreatedSrm))]
        public int? CreatedBySrm { get; set; }
        public SalesRepManager? CreatedSrm { get; set; }

        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }

        // NEW FIELD
        public bool IsApprovedByManager { get; set; } = false;
    }

    public class CommunicationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime LogDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Lead))]
        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }

        [ForeignKey(nameof(Deal))]
        public int? DealId { get; set; }
        public Deal? Deal { get; set; }

        [ForeignKey(nameof(SalesRep))]
        public int? SalesRepId { get; set; }
        public SalesRep? SalesRep { get; set; }
    }
}
