namespace CRMWepApi.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class AssignSalesRepDto
    {
        public int ManagerId { get; set; }
        public int SalesRepManagerId { get; set; }
        public List<int> SalesRepIds { get; set; }
    }
}
