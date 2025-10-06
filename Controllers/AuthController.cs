using CRMWepApi.DTOs;
using CRMWepApi.Models;
using CRMWepApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public IActionResult Login([FromBody] LoginRequestDto request)
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

        // just for role tesT
        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                userId = User.FindFirst("id")?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
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

  
}
