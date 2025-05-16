using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
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
    }
}
