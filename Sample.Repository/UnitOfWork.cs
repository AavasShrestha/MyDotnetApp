using Sample.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Sample.Data.RoutingDB;

namespace Sample.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RoutingDbContext _dbContext;
        private Repository<User> _userRepository;
        private Repository<tblClientDetails> _clientDetailsRepository;
        private Repository<Logo> _logoRepository;
        private Repository<RegisterDb> _registerDbRepository;

        public RoutingDbContext DbContext => _dbContext; // <-- expose it
        public UnitOfWork(RoutingDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");
        }

        public IRepository<User> UserRepository =>
           _userRepository ?? (_userRepository = new Repository<User>(_dbContext));

        public IRepository<tblClientDetails> ClientDetailsRepository =>
            _clientDetailsRepository ?? (_clientDetailsRepository = new Repository<tblClientDetails>(_dbContext));

        public IRepository<Logo> LogoRepository =>
            _logoRepository ?? (_logoRepository = new Repository<Logo>(_dbContext));

        public IRepository<RegisterDb> RegisterDbRepository =>
            _registerDbRepository ?? (_registerDbRepository = new Repository<RegisterDb>(_dbContext));

        public bool Commit()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    SaveChanges(_dbContext);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private int SaveChanges(RoutingDbContext _dbContext)
        {
            return _dbContext.SaveChanges();
        }

        public List<T> ExecuteStoredProcedure<T>(string storedProcedure, Func<DbDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    var results = new List<T>();
                    var properties = typeof(T).GetProperties();

                    while (reader.Read())
                    {
                        T item = Activator.CreateInstance<T>();

                        foreach (var property in properties)
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(item, reader[property.Name]);
                            }
                        }

                        results.Add(item);
                    }
                    return results;
                }
            }
        }

        public List<List<T>> ExecuteStoredProcedureMultipleResults<T>(string storedProcedure, Func<DbDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                var allResults = new List<List<T>>(); // Outer list to hold all result sets

                using (var reader = command.ExecuteReader())
                {
                    do
                    {
                        var singleResultSet = new List<T>(); // Inner list for each result set

                        while (reader.Read())
                        {
                            singleResultSet.Add(map(reader)); // Map each row to the generic type T
                        }

                        allResults.Add(singleResultSet); // Add the result set to the outer list
                    } while (reader.NextResult()); // Move to the next result set
                }

                return allResults;
            }
        }

        public List<List<object>> ExecuteStoredProcedureMultipleResults(string storedProcedure, params SqlParameter[] parameters)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                var allResults = new List<List<object>>(); // Outer list for all result sets

                using (var reader = command.ExecuteReader())
                {
                    do
                    {
                        var singleResultSet = new List<object>(); // Inner list for each result set

                        while (reader.Read())
                        {
                            var row = new object[reader.FieldCount];
                            reader.GetValues(row); // Get all values in the current row
                            singleResultSet.Add(row); // Add the row to the result set
                        }

                        allResults.Add(singleResultSet); // Add the result set to the outer list
                    } while (reader.NextResult()); // Move to the next result set
                }

                return allResults;
            }
        }
    }
}