using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<string> GetAllDatabases()
        {
            var databases = new List<string>();

            // get the db connection from your UnitOfWork's DbContext
            var connection = _unitOfWork.DbContext.Database.GetDbConnection();

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT name FROM sys.databases WHERE database_id > 4";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            databases.Add(reader.GetString(0));
                        }
                    }
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return databases;
        }

    }
}
