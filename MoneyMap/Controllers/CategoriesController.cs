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
        public async Task<IActionResult> Add([FromBody] CategoryPost category)
        {
            _context.Add(new Categories()
            {
                IdCategory = Guid.NewGuid(),
                Title = category.Title,
                IsInput = category.IsInput,
                DateRegistered = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }



        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] CategoryPost category)
        {
            Categories thisCategory = await _context
                .Categories
                .FirstOrDefaultAsync(p => p.IdCategory == category.IdCategory);

            if (thisCategory == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            thisCategory.Title = category.Title;
            thisCategory.IsInput = category.IsInput;
            _context.Update(thisCategory);

            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            Categories thisCategory = await _context
                .Categories
                .FirstOrDefaultAsync(p => p.IdCategory == id);

            if (thisCategory == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            thisCategory.IsDeleted = true;
            _context.Update(thisCategory);

            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }
    }
}