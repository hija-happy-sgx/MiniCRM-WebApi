using CRMWepApi.Enums;
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


    public class CreateLeadDto
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
