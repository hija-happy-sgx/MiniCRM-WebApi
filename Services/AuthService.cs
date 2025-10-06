using CRMWepApi.Data;
using CRMWepApi.Enums;
using CRMWepApi.Models;
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

            //0. CHeck Admin

            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.PasswordHash == password);
            if (admin != null) 
            { user = admin;
                role = UserRole.Admin.ToString();
            }


            // 1. Check Managers
            var manager = _context.Managers.FirstOrDefault(m => m.Email == email && m.Password == password);
            if (manager != null)
            {
                user = manager;
                role = UserRole.Manager.ToString(); ;
            }
            // 2. Check SalesRepManagers
            else
            {
                var srm = _context.SalesRepManagers.FirstOrDefault(s => s.Email == email && s.PasswordHash == password);
                if (srm != null)
                {
                    user = srm;
                    role = UserRole.SalesRepManager.ToString();
                }
                // 3. Check SalesReps
                else
                {
                    var rep = _context.SalesReps.FirstOrDefault(r => r.Email == email && r.PasswordHash == password);
                    if (rep != null)
                    {
                        user = rep;
                        role = UserRole.SalesRep.ToString();
                    }
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
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
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
