using CRMWepApi.Models;

namespace CRMWebApi.DTOs
{
    public class DealDto
    {
        public int DealId { get; set; }
        public int LeadId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public Deal Status { get; set; } // Enum
        public int StageId { get; set; }
    }

    public class CreateDealDto
    {
        public int LeadId { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
    }
}
