namespace CRMWepApi.DTOs
{
  

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }

    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }  // Manager / SalesRepManager / SalesRep
    }

    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // "Manager", "SalesRepManager", "SalesRep"
    }

}
