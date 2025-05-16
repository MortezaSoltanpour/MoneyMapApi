using System.Net;

namespace MoneyMap.Models.Dtos
{
    public class ApiResult
    {
        public DateTime DateRetrieve { get; set; } = DateTime.Now;
        public bool IsDataFromCached { get; set; } = true;
        public bool IsSuccess { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? PayLoad { get; set; }
        public int PageCount { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public int TotalItems { get; set; } = 1;
    }
}
