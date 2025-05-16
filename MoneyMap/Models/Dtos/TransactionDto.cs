namespace MoneyMap.Models.Dtos
{
    public class TransactionDto
    {
        public Guid IdTransaction { get; set; }
        public DateTime DateRegistered { get; set; }
        public required string Description { get; set; }
        public double Amount { get; set; }
        public string FileAttached { get; set; }
        public Guid IdCategory { get; set; }
        public string Category { get; set; }
    }

    public class TransactionPostDto
    {
        public Guid? IdTransaction { get; set; }
        public required string Description { get; set; }
        public double Amount { get; set; }
        public string? FileAttached { get; set; }
        public Guid IdCategory { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
