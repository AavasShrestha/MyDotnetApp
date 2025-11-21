using System.Threading.Tasks;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;

namespace Sample.Service.Service.BranchService
{
    public interface IBranchService
    {
        Task<int> TotalBranchCount();
        Task<List<Branch>> GetAllBranches();
    }
}
