namespace CRMWepApi.DTOs
{
    public class CommunicationLogDto
    {
        public string Type { get; set; }           // Call / Email / Meeting
        public string Notes { get; set; }
        public DateTime LogDate { get; set; }
        public int SalesRepId { get; set; }
        public string TargetType { get; set; }     // "Lead" or "Deal"
        public int TargetId { get; set; }          // LeadId or DealId
    }
}
