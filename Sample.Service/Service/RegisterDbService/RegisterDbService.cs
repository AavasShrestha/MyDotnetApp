using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Sample.Data.DTO;
using Sample.Repository;
using Sample.Service.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Data.RoutingDB;


namespace Sample.Service.Service.RegisterDbService
{
    public class RegisterDbService : IRegisterDbService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDatabaseService _databaseService;
        private readonly IConfiguration _configuration;

        public RegisterDbService(IUnitOfWork unitOfWork, IDatabaseService databaseService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public IEnumerable<RegisterDbDto> GetAllDatabases()
        {
            var databases = _unitOfWork.RegisterDbRepository.GetQuerable().ToList();
            //var dbDtos = databases.Select(entity => new RegisterDbDto
            //{
            //    Id = entity.Id,
            //    Project_name = entity.Project_name,
            //    Db_name = entity.Db_name,
            //    isActive = entity.isActive
            //}).ToList();

            return databases.Select(entity => new RegisterDbDto
            {
                Id = entity.Id,
                Project_name = entity.Project_name,
                Db_name = entity.Db_name,
                isActive = entity.isActive
            }).ToList();
        }

        public RegisterDbDto GetDatabaseById(int id)
        {
            var entity = _unitOfWork.RegisterDbRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
            if (entity == null) return null;

            return new RegisterDbDto
            {
                Id = entity.Id,
                Project_name = entity.Project_name,
                Db_name = entity.Db_name,
                isActive = entity.isActive
            };
        }
        public ValidationDTO CreateDatabase(int userID, RegisterDbDto registerDbDto)
        {
            var response = new ValidationDTO();
           
            if(registerDbDto == null)
            {
                response.IsSuccess = false;
                response.Message = "Database cannot be null or empty";
                return response;
            }


            // check duplicate registerdb table
            var exists = _unitOfWork.RegisterDbRepository.GetQuerable(x => x.Db_name == registerDbDto.Db_name).Any();
            if (exists)
            {
                response.IsSuccess = false;
                response.Message = "Database already exists";
                return response;
            }

            //check duplicate in sql server

            var existingdb = _databaseService.GetAllDatabases();
            if (existingdb.Contains(registerDbDto.Db_name))
            {
                response.IsSuccess = false;
                response.Message = "Database already exists in sql server";
                return response;
            }

            // Insert Record into RegisterDb table
            var entity = new RegisterDb
            {
                Project_name = registerDbDto.Project_name,
                Db_name = registerDbDto.Db_name,
                isActive = registerDbDto.isActive
            };
            _unitOfWork.RegisterDbRepository.Add(entity);
            _unitOfWork.Commit();

            // Create Physical Database in SQL Server

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand($"CREATE DATABASE [{registerDbDto.Db_name}]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            response.IsSuccess = true;
            response.Message = $"Database '{registerDbDto.Db_name}' created successfully and registered.";
            return response;

        }

        

      

        public RegisterDbDto PatchDatabase(int userID, int id, Dictionary<string, object> patchData)
        {
            throw new NotImplementedException();
        }

        public ValidationDTO UpdateDatabase(int userId, int id, RegisterDbDto registerDbDto)
        {
            throw new NotImplementedException();
        }
        public ValidationDTO DeleteDatabase(int id)
        {
            throw new NotImplementedException();
        }
    }
}
