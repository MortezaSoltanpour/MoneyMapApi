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
    public class CategoriesController : BaseController
    {
        public CategoriesController(ApplicationDbContext context) : base(context)
        {
        }


        [HttpGet("All")]
        public async Task<IActionResult> AllUsers()
        {
            List<CategoriesDto> categories = await _context
                .Categories
                .Select(p => new CategoriesDto()
                {
                    IdCategory = p.IdCategory,
                    DateRegistered = p.DateRegistered,
                    Title = p.Title,
                    IsInput = p.IsInput
                })
                .ToListAsync();

            return ReturnResponse(categories, HttpStatusCode.OK, null);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CategoryPost user)
        {
            _context.Add(new Categories()
            {
                IdCategory = Guid.NewGuid(),
                Title = user.Title,
                IsInput = user.IsInput,
            });
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }



    }
}