using Microsoft.EntityFrameworkCore;

namespace TODO;

public class Database : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        contextOptionsBuilder.UseSqlite("Data source=database.db");
    }

    public DbSet<Signup> Signups { get; set; }
}