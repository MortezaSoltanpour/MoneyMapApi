namespace MoneyMap.Models.Dtos
{
    public class UserInputDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class NewUserInputDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Fullname { get; set; }
    }

    public class EditUserInputDto
    {
        public required string Email { get; set; }
        public required string Fullname { get; set; }

    }
    public class LoginResponse
    {
        public string JWT { get; set; }
    }

    public class UserDto
    {
        public Guid IdUser { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool IsDeleted { get; set; }
    }
}
