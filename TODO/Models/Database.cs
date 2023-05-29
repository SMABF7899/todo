using Microsoft.EntityFrameworkCore;
using TODO.Models;

namespace TODO;

public class Database : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        contextOptionsBuilder.UseSqlite("Data source=database.db");
    }

    public DbSet<Signup> Signups { get; set; }
    public DbSet<Issue> Issues { get; set; }
}