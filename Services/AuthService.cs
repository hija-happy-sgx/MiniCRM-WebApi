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
            Console.WriteLine($"🔍 STEP 3: Starting authentication process for: {email}");

            var hashedPassword = PasswordHelper.HashPassword(password);
            Console.WriteLine($"   - Password hashed: {hashedPassword.Substring(0, 20)}...");


            user = null;
            role = null;


            //0. CHeck Admin
            Console.WriteLine($"   - Checking Admin table...");
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.PasswordHash == password);
            if (admin != null)
            {
                if (admin.PasswordHash == password || admin.PasswordHash == hashedPassword)
                {
                    {
                        user = admin;

                        role = UserRole.Admin.ToString();

                        Console.WriteLine($"STEP 4: Admin user found - ID: {admin.AdminId}");
                    }
                }
            }

            // 1. Check Managers
            if (user == null)
            {
                Console.WriteLine($"   - Checking Manager table...");
                var manager = _context.Managers.FirstOrDefault(m => m.Email == email);
                if (manager != null && (manager.Password == password || manager.Password == hashedPassword))
                {
                    user = manager;
                    role = UserRole.Manager.ToString();
                    Console.WriteLine($"✅ STEP 4: Manager user found - ID: {manager.ManagerId}");
                }
            }
            // 2. Check SalesRepManagers
            if (user == null)
            {
                Console.WriteLine($"   - Checking SalesRepManager table...");
                var srm = _context.SalesRepManagers.FirstOrDefault(s => s.Email == email && s.PasswordHash == hashedPassword);
                if (srm != null)
                {
                    user = srm;
                    role = UserRole.SalesRepManager.ToString(); Console.WriteLine($"✅ STEP 4: SalesRepManager user found - ID: {srm.SrmId}");
                }
            }
            // 3. Check SalesReps
            if (user == null)
            {
                Console.WriteLine($"   - Checking SalesRep table...");
                var rep = _context.SalesReps.FirstOrDefault(r => r.Email == email && r.PasswordHash == hashedPassword);
                if (rep != null)
                {
                    user = rep;
                    role = UserRole.SalesRep.ToString();
                    Console.WriteLine($"✅ STEP 4: SalesRep user found - ID: {rep.SalesRepId}");
                }
            }

            if (user != null)
            {
                var token = GenerateToken(user, role);
                Console.WriteLine($"🎫 STEP 5: JWT Token generated successfully");
                Console.WriteLine($"   - Role in token: {role}");
                Console.WriteLine($"   - User ID in token: {GetUserId(user)}");
                Console.WriteLine($"   - Token (first 50 chars): {token.Substring(0, Math.Min(50, token.Length))}...");
                return token;
            }
            else
            {
                Console.WriteLine($"❌ STEP 4: No user found with provided credentials");
                return null;
            }
        }

        //    if (user == null) return null;

        //    // Generate JWT
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //        new Claim("id", GetUserId(user).ToString()),
        //        //new Claim(ClaimTypes.Role, role)
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(8),

        //        Issuer = _config["Jwt:Issuer"],
        //        Audience = _config["Jwt:Audience"],

        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}


        //private string GenerateToken(object user, string role)
        //{
        //    Console.WriteLine($"🔧 STEP 6: Generating JWT token...");
        //    Console.WriteLine($"   - For role: {role}");
        //    Console.WriteLine($"   - For user ID: {GetUserId(user)}");

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        //    Console.WriteLine(key);

        //    var claims = new Claim[]
        //    {
        //new Claim("id", GetUserId(user).ToString()),
        //new Claim(ClaimTypes.Role, role)
        //    };

        //    Console.WriteLine($"   - Claims added to token:");
        //    foreach (var claim in claims)
        //    {
        //        Console.WriteLine($"     * {claim.Type}: {claim.Value}");
        //    }

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddHours(8),
        //        Issuer = _config["Jwt:Issuer"],
        //        Audience = _config["Jwt:Audience"],
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);

        //    Console.WriteLine($"✅ STEP 7: Token created successfully - Length: {tokenString.Length} chars");

        //    return tokenString;
        //}



        //private string GenerateToken(object user, string role)
        //{
        //    var claims = new[]

        //    {

        //          new Claim(ClaimTypes.Name,"hija" ),
        //          new Claim(ClaimTypes.Role,role),
        //          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        //        };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(

        //        issuer: _config["Jwt:Issuer"],

        //        audience: _config["Jwt:Audience"],

        //        claims: claims,

        //        expires: DateTime.Now.AddHours(1),

        //        signingCredentials: creds

        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);

        //}

        private string GenerateToken(object user, string role)
        {
            var userId = GetUserId(user);
            var email = (string)user.GetType().GetProperty("Email")?.GetValue(user, null);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Email, email ?? ""),
        new Claim(ClaimTypes.Role, role),
        new Claim("role",role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public int GetUserId(object user)
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
