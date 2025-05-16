using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
using MoneyMap.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyMap.Utility.Helper
{
    public static class SecurityHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string GenerateJSONWebToken(UserDto userInfo, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, userInfo.IdUser.ToString()),
                new Claim(ClaimTypes.UserData, userInfo.Email),
                new Claim(ClaimTypes.Role, "admin"),
            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static async Task<Users> GetCurrentUser(HttpContext request, ApplicationDbContext _context)
        {
            var claimsIdentity = request.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            Guid userid = Guid.Parse(userDataClaim.Value);
            Users user = await _context
                .Users
                .SingleAsync(p => p.IdUser == userid);

            return user;
        }

        public static Guid? GetCurrentUserId()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            return Guid.Parse(user?.FindFirst(ClaimTypes.Sid)?.Value);
        }
    }
}
