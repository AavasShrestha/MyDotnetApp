using Microsoft.EntityFrameworkCore;
using Sample.Data.KamanaDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.KamanaDB
{
    public class KamanaDbContext : DbContext
    {
        public KamanaDbContext(DbContextOptions<KamanaDbContext> options
            ) : base(options)
        {
            
        }
        public DbSet<TblAppMenu> TblAppMenus { get; set; }
    }
}
