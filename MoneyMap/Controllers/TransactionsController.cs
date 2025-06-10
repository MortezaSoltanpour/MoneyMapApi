using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
using MoneyMap.Models.Entities;
using System.Net;
using System.Transactions;

namespace MoneyMap.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : BaseController
    {
        public TransactionsController(ApplicationDbContext context) : base(context)
        {
        }

        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery] DateTime? dtStart, [FromQuery] DateTime? dtEnd, [FromQuery] Guid[]? idCategory)
        {
            if (dtStart == null)
                dtStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (dtEnd == null)
                dtEnd = dtStart.Value.AddMonths(1).AddDays(-1);

            List<TransactionDto> transactions = await _context
                .Transactions
                .Where(p => p.DateRegistered >= dtStart && p.DateRegistered < dtEnd)
                .Select(p => new TransactionDto()
                {
                    IdTransaction = p.IdTransaction,
                    DateRegistered = p.DateRegistered,
                    Description = p.Description,
                    Amount = p.Amount,
                    Category = p.Category.Title,
                    IdCategory = p.CategoryId,
                    FileAttached = p.FileAttached,
                    IsInput = p.Category.IsInput
                })
                .OrderBy(p => p.DateRegistered)
                .ToListAsync();

            if (idCategory != null && idCategory.Length > 0)
                transactions = transactions.Where(p => idCategory.Any(c => c == p.IdCategory)).ToList();

            return ReturnResponse(transactions, HttpStatusCode.OK, null);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] TransactionPostDto transaction)
        {
            string fileName = "";
            if (transaction.File != null && transaction.File.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(transaction.File.FileName);
                var filePath = Path.Combine("wwwroot/Uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await transaction.File.CopyToAsync(stream);
                }
            }
            _context.Add(new Transactions()
            {
                IdTransaction = Guid.NewGuid(),
                Description = transaction.Description,
                CategoryId = transaction.IdCategory,
                DateRegistered = transaction.DateRegistered,
                Amount = transaction.Amount,
                FileAttached = fileName
            });


            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] TransactionPostDto transaction)
        {
            Transactions thisTransaction = await _context
                .Transactions
                .FirstOrDefaultAsync(p => p.IdTransaction == transaction.IdTransaction);

            if (thisTransaction == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            thisTransaction.Description = transaction.Description;
            thisTransaction.Amount = transaction.Amount;
            thisTransaction.CategoryId = transaction.IdCategory;
            thisTransaction.DateRegistered = transaction.DateRegistered;
            _context.Update(thisTransaction);

            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Transactions thisTransaction = await _context
                .Transactions
                .FirstOrDefaultAsync(p => p.IdTransaction == id);

            if (thisTransaction == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            thisTransaction.IsDeleted = true;
            _context.Update(thisTransaction);

            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            TransactionDto transaction = await _context
                .Transactions
                .Select(p => new TransactionDto()
                {
                    IdTransaction = p.IdTransaction,
                    DateRegistered = p.DateRegistered,
                    Description = p.Description,
                    Amount = p.Amount,
                    Category = p.Category.Title,
                    IdCategory = p.CategoryId,
                    FileAttached = $"{Request.Scheme}://{Request.Host}/uploads/{p.FileAttached}",
                    IsInput = p.Category.IsInput
                })
                .FirstOrDefaultAsync(p => p.IdTransaction == id);

            if (transaction == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            return ReturnResponse(transaction, HttpStatusCode.OK, null);
        }
    }
}