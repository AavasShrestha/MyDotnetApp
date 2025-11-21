using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sample.Data.DTO;
using Sample.Data;
using Sample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Data.RoutingDB;

namespace Sample.Service.Service.BranchService
{
    public class BranchService : IBranchService
    {
        private readonly RoutingDbContext _context;

        public BranchService(RoutingDbContext context)
        {
            _context = context;
        }

        public Task<List<Branch>> GetAllBranches()
        {
            return  _context.tblBranch.ToListAsync();
        }

        public async Task<int> TotalBranchCount()
        {
            return await _context.tblBranch.CountAsync();
        }
    }
}
    

