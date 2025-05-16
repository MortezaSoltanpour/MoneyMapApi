namespace MoneyMap.Models.Dtos
{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        public string JWT { get; set; }
    }
}
