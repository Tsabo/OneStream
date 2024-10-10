using Microsoft.EntityFrameworkCore;

namespace OneStream.Api.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Person> People => Set<Person>();
    }
}
