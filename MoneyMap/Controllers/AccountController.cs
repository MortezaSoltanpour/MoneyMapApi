using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
using MoneyMap.Models.Entities;
using MoneyMap.Utility;
using System.Net;

namespace MoneyMap.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext context) : base(context)
        {
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
               );
             
            if (user == null)
            {
                return ReturnResponse(null, HttpStatusCode.Forbidden, new List<string>() { "username and/or password is incorrect" });
            }

            // todo : Generate JWT Token
            LoginResponse result = new LoginResponse()
            {
                JWT = "Sample Token"
            };
            return ReturnResponse(result, HttpStatusCode.OK, null);
        }
    }
}
