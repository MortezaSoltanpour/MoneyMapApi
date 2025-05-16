using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMap.Models;
using MoneyMap.Models.Dtos;
using System.Net;

namespace MoneyMap.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        internal readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        internal IActionResult ReturnResponse(object? data,
        HttpStatusCode status,
        List<string>? errors)
        {
            ApiResult result = new ApiResult()
            {
                StatusCode = status,
                PayLoad = data,
                IsSuccess = status == HttpStatusCode.OK || status == HttpStatusCode.Created,
                ErrorMessages = errors,
                IsDataFromCached = false
            };

            if (data is System.Collections.ICollection collection)
                result.TotalItems = collection.Count;

            return StatusCode((int)status, result);
        }

    }
}
