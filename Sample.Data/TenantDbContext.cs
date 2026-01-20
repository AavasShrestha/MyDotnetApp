using Microsoft.EntityFrameworkCore;
using Sample.Data.RoutingDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }


        public DbSet<User> Users => Set<User>();
        public DbSet<tblClientDetails> Client => Set<tblClientDetails>();
       
    }
}
