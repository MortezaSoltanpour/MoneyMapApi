using Microsoft.EntityFrameworkCore;

namespace MoneyMap.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
