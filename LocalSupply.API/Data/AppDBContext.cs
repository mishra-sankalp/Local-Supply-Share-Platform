using LocalSupply.API.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LocalSupply.API.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     
    // }
}