using Microsoft.EntityFrameworkCore;
using Sample.Data.TenantDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
         : base(options)
        { }

        public DbSet<Tenant> Tenants => Set<Tenant>();
    }

   
}
