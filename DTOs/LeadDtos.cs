using CRMWepApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRMWepApi.DTOs
{
    //public class LeadDto
    //{
    //    public int LeadId { get; set; }
    //    public string Name { get; set; }
    //    public string Company { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public string Status { get; set; } // Enum → String
    //    public int AssignedToUserId { get; set; }
    //    public int CreatedByManagerId { get; set; }
    //}

    public class LeadDto
    {
        public int LeadId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Source { get; set; }
        public LeadStatus? Status { get; set; }

        public int? AssignedToSalesRep { get; set; }
        public string? SalesRepName { get; set; }

        public int? AssignedToSrm { get; set; }
        public string? SrmName { get; set; }

        public int? CreatedByManager { get; set; }
        public string? ManagerName { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class LeadCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LeadStatus? Status { get; set; } // optional, default to New
    }

    public class LeadDashboardDto
    {
        public int TotalLeads { get; set; }
        public int NewLeads { get; set; }
        public int ContactedLeads { get; set; }
        public int ConvertedLeads { get; set; }
        public int DisqualifiedLeads { get; set; }
        public int QualifiedLeads { get; set; }

    }


    public class CreateLeadDto
    {
        public int LeadId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int Value { get; set; }
        public int? AssignedToSalesRepId { get; set; }
        public int StageId { get; set; }
        public DateTime CreatedAt = DateTime.UtcNow;
        public DateTime ExpectedCloseDate { get; set; }
    } 

    public class LeadStatusUpdateModel
    {
        [Required]
        public LeadStatus Status { get; set; }
    }

    public class OpportunityCreateModel
    {
        public int LeadId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }


}
