using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
using MoneyMap.Models.Entities;
using MoneyMap.Utility.Helper;
using System.Net;

namespace MoneyMap.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        IConfiguration _config;
        public AccountController(ApplicationDbContext context, IConfiguration config) : base(context)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("LoginRequest")]
        public async Task<IActionResult> LoginRequest([FromBody] LoginDto login)
        {
            login.Email = login.Email.ToLower();

            Users user = await _context
                .Users
                .SingleOrDefaultAsync(p =>
               p.Email == login.Email &&
               p.Password == PasswordHelper.EncodePasswordMd5(login.Password)
               )
                ;

            if (user == null)
            {
                return ReturnResponse(null, HttpStatusCode.Forbidden, new List<string>() { "username and/or password is incorrect" });
            }

            UserDto userInfo = new UserDto()
            {
                Email = user.Email,
                Fullname = user.Email,
                DateRegistered = user.DateRegistered,
                IsDeleted = user.IsDeleted,

            };
            string token = SecurityHelper.GenerateJSONWebToken(userInfo, _config);
            LoginResponse result = new LoginResponse()
            {
                JWT = token
            };
            return ReturnResponse(result, HttpStatusCode.OK, null);
        }
    }
}