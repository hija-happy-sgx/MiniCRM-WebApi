namespace CRMWepApi.DTOs
{
    public class LeadDto
    {
        public int LeadId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; } // Enum → String
        public int AssignedToUserId { get; set; }
        public int CreatedByManagerId { get; set; }
    }

    public class CreateLeadDto
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
