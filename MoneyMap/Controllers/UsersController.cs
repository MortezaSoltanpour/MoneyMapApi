﻿using Microsoft.AspNetCore.Authorization;
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
                IdUser = user.IdUser,
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
                    IdUser = p.IdUser,
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
                DateRegistered = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] EditUserInputDto user)
        {
            var idUser = SecurityHelper.GetCurrentUserId();
            Users thisUser = await _context.Users.FirstAsync(p => p.IdUser == idUser);
            user.Email = user.Email.ToLower();
            if (_context.Users.Any(p => p.Email == user.Email && p.IdUser != idUser))
                return ReturnResponse(null, HttpStatusCode.Conflict, new List<string>() { "Email is already exist" });

            thisUser.Fullname = user.Fullname;
            thisUser.Email = user.Email;
            _context.Update(thisUser);
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto user)
        {
            var idUser = SecurityHelper.GetCurrentUserId();
            Users thisUser = await _context.Users.FirstAsync(p => p.IdUser == idUser);

            if (thisUser.Password != PasswordHelper.EncodePasswordMd5(user.OldPassword))
                return ReturnResponse(null, HttpStatusCode.Forbidden, new List<string>() { "Old password is not correct" });

            thisUser.Password = PasswordHelper.EncodePasswordMd5(user.NewPassword);
            _context.Update(thisUser);
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }
    }
}