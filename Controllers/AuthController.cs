using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMWepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Email and password are required." });

            // Authenticate user and detect role automatically
            object user;
            string role;
            var token = _authService.Authenticate(request.Email, request.Password, out user, out role);

            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new
            {
                token,
                role,
                user_id = GetUserId(user)
            });
        }

        private int GetUserId(object user)
        {
            return user switch
            {
                Admin a => a.AdminId,
                Manager m => m.ManagerId,
                SalesRepManager s => s.SrmId,
                SalesRep r => r.SalesRepId,
                _ => 0
            };
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
