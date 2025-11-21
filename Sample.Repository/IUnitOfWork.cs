
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Sample.Data.RoutingDB;
using Sample.Data;

namespace Sample.Repository
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<tblClientDetails> ClientDetailsRepository { get; }
        IRepository<Logo> LogoRepository { get; }
        RoutingDbContext DbContext { get; }
        IRepository<RegisterDb> RegisterDbRepository { get; }
        

        bool Commit();

        List<T> ExecuteStoredProcedure<T>(string storedProcedure, Func<DbDataReader, T> map, params SqlParameter[] parameters);

        List<List<T>> ExecuteStoredProcedureMultipleResults<T>(string storedProcedure, Func<DbDataReader, T> map, params SqlParameter[] parameters);

        List<List<object>> ExecuteStoredProcedureMultipleResults(string storedProcedure, params SqlParameter[] parameters);
    }
}