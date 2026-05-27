using LocalSupply.API.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LocalSupply.API.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<ListingRequest> Requests => Set<ListingRequest>();
    public DbSet<CreditTransaction> CreditTransactions => Set<CreditTransaction>();
}