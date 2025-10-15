using CRMWepApi.Data;
using CRMWepApi.Enums;
using CRMWepApi.Models;
using CRMWepApi.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CRMWepApi.Services
{
    public class AuthService
    {
        private readonly CrmDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(CrmDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Authenticate user by email + password and automatically detect role.
        /// Returns JWT token if successful, null if invalid.
        /// </summary>
        public string Authenticate(string email, string password, out object user, out string role)
        {
            user = null;
            role = null;


            var hashedPassword = PasswordHelper.HashPassword(password);

            //0. CHeck Admin

            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.PasswordHash == password);
            if (admin != null)
            {
                if (admin.PasswordHash == password || admin.PasswordHash == hashedPassword)
                {
                    {
                        user = admin;
                        role = UserRole.Admin.ToString();
                    }
                }

            }

            // 1. Check Managers
            if (user == null)
            {
                var manager = _context.Managers.FirstOrDefault(m => m.Email == email);
                if (manager != null && (manager.Password == password || manager.Password == hashedPassword))
                {
                    user = manager;
                    role = UserRole.Manager.ToString();
                }
            }
            // 2. Check SalesRepManagers
            if (user == null)
                {
                    var srm = _context.SalesRepManagers.FirstOrDefault(s => s.Email == email && s.PasswordHash == hashedPassword);
                    if (srm != null)
                    {
                        user = srm;
                        role = UserRole.SalesRepManager.ToString();
                    }
                }
                // 3. Check SalesReps
                if (user == null)
                {
                    var rep = _context.SalesReps.FirstOrDefault(r => r.Email == email && r.PasswordHash == hashedPassword);
                    if (rep != null)
                    {
                        user = rep;
                        role = UserRole.SalesRep.ToString();
                    }
                }

                if (user == null) return null;

                // Generate JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("id", GetUserId(user).ToString()),
                    //new Claim(ClaimTypes.Role, role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),

                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"],

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        
        private int GetUserId(object user)
        {
            return user switch
            {
                Admin a =>a.AdminId,
                Manager m => m.ManagerId,
                SalesRepManager s => s.SrmId,
                SalesRep r => r.SalesRepId,
                _ => 0
            };
        }
    }
}
