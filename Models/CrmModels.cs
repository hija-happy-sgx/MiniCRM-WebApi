using CRMWepApi.Enums;

namespace CRMWepApi.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }


    public class Manager
    {
        public int ManagerId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public ICollection<SalesRepManager>? SalesRepManagers { get; set; }
        public ICollection<Lead>? LeadsCreated { get; set; }
        public ICollection<Deal>? DealsCreated { get; set; }
    }

    public class SalesRepManager
    {
        public int SrmId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public ICollection<SalesRep>? SalesReps { get; set; }
        public ICollection<Lead>? LeadsCreated { get; set; }
        public ICollection<Deal>? DealsCreated { get; set; }
    }

    public class SalesRep
    {
        public int SalesRepId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int SrmId { get; set; }
        public SalesRepManager? SalesRepManager { get; set; }

        public ICollection<Lead>? Leads { get; set; }
        public ICollection<Deal>? Deals { get; set; }
        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }
    }

    public class PipelineStage
    {
        public int StageId { get; set; }
        public string StageName { get; set; } = null!;
        public int StageOrder { get; set; }

        public ICollection<Deal>? Deals { get; set; }
    }

    public class Lead
    {
        public int LeadId { get; set; }
        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        public LeadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int? AssignedToSalesRep { get; set; }
        public SalesRep? SalesRep { get; set; }

        public int? AssignedToSrm { get; set; }
        public SalesRepManager? AssignedSrm { get; set; }

        public int? CreatedByManager { get; set; }
        public Manager? Manager { get; set; }

        public int? CreatedBySrm { get; set; }
        public SalesRepManager? CreatedSrm { get; set; }

        public ICollection<Deal>? Deals { get; set; }
        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }
    }

    public class Deal
    {
        public int DealId { get; set; }
        public string DealName { get; set; } = null!;
        public decimal Value { get; set; }
        public DealStatus Status { get; set; } 

        public DateTime? ExpectedCloseDate { get; set; }
        public DateTime? ActualCloseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int LeadId { get; set; }
        public Lead? Lead { get; set; }

        public int StageId { get; set; }
        public PipelineStage? Stage { get; set; }

        public int? AssignedToSalesRep { get; set; }
        public SalesRep? SalesRep { get; set; }

        public int? AssignedToSrm { get; set; }
        public SalesRepManager? AssignedSrm { get; set; }

        public int? CreatedByManager { get; set; }
        public Manager? Manager { get; set; }

        public int? CreatedBySrm { get; set; }
        public SalesRepManager? CreatedSrm { get; set; }

        public ICollection<CommunicationLog>? CommunicationLogs { get; set; }
    }

    public class CommunicationLog
    {
        public int LogId { get; set; }
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime LogDate { get; set; }

        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }

        public int? DealId { get; set; }
        public Deal? Deal { get; set; }

        public int? SalesRepId { get; set; }
        public SalesRep? SalesRep { get; set; }
    }
}
