using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class WedPlannerContext : DbContext
    {
        public WedPlannerContext(DbContextOptions<WedPlannerContext> options) : base(options) { }
        public DbSet<User> UserTable {get;set;}
        public DbSet<Wedding> WeddingTable {get;set;}
        public DbSet<WeddingUser> WeddingGuest {get; set;}
    }
}