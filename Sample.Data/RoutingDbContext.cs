using Microsoft.EntityFrameworkCore;
using Sample.Data.RoutingDB;

namespace Sample.Data
{
    public class RoutingDbContext : DbContext
    {

        public DbSet<tblClientDetails> tblClientDetails { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Logo> Logo { get; set; }
        public DbSet<RegisterDb> RegisterDb { get; set; }
        public RoutingDbContext(DbContextOptions<RoutingDbContext> options) : base(options)
        {
        }
    }
}