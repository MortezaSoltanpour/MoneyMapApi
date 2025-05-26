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
        public async Task<IActionResult> All([FromQuery] DateTime? dtStart, [FromQuery] DateTime? dtEnd, [FromQuery] Guid? idCategory)
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
                    FileAttached = p.FileAttached
                })
                .ToListAsync();

            if (idCategory != null)
                transactions = transactions.Where(p => p.IdCategory == idCategory).ToList();

            return ReturnResponse(transactions, HttpStatusCode.OK, null);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TransactionPostDto transaction)
        {
            _context.Add(new Transactions()
            {
                IdTransaction = Guid.NewGuid(),
                Description = transaction.Description,
                CategoryId = transaction.IdCategory,
                DateRegistered = transaction.DateCreated,
                Amount = transaction.Amount,

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
            thisTransaction.DateRegistered = transaction.DateCreated;
            _context.Update(thisTransaction);

            await _context.SaveChangesAsync();

            return ReturnResponse(null, HttpStatusCode.OK, null);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete( Guid id)
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
                    FileAttached = p.FileAttached
                })
                .FirstOrDefaultAsync(p => p.IdTransaction == id);

            if (transaction == null)
                return ReturnResponse(null, HttpStatusCode.NotFound, null);

            return ReturnResponse(transaction, HttpStatusCode.OK, null);
        }
    }
}