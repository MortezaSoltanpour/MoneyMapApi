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
    public class UsersController : BaseController
    {
        IConfiguration _config;
        public UsersController(ApplicationDbContext context, IConfiguration config) : base(context)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("LoginRequest")]
        public async Task<IActionResult> LoginRequest([FromBody] UserInputDto login)
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

        [HttpGet("AllUsers")]
        public async Task<IActionResult> AllUsers()
        {
            List<UserDto> users = await _context
                .Users
                .Select(p => new UserDto()
                {
                    DateRegistered = p.DateRegistered,
                    Email = p.Email,
                    Fullname = p.Fullname,
                    IsDeleted = p.IsDeleted,
                })
                .ToListAsync();

            return ReturnResponse(users, HttpStatusCode.OK, null);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] NewUserInputDto user)
        {
            user.Email = user.Email.ToLower();

            if (_context.Users.Any(p => p.Email == user.Email))
                return ReturnResponse(null, HttpStatusCode.Conflict, new List<string>() { "Email is already exist" });

            _context.Add(new Users()
            {
                Email = user.Email,
                IdUser = Guid.NewGuid(),
                Fullname = user.Fullname,
                Password = PasswordHelper.EncodePasswordMd5(user.Password),
            });
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }
    }
}