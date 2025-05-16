namespace MoneyMap.Models.Dtos
{
    public class CategoriesDto
    {
        public Guid IdCategory { get; set; }
        public required string Title { get; set; }
        public bool IsInput { get; set; }
        public DateTime DateRegistered { get; set; }
    }

    public class CategoryPost
    {
        public Guid? IdCategory { get; set; }
        public required string Title { get; set; }
        public bool IsInput { get; set; }
    }

}