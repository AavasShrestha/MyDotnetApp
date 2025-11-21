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
using Microsoft.EntityFrameworkCore;


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

            if (registerDbDto == null)
            {
                response.IsSuccess = false;
                response.Message = "Database cannot be null or empty";
                return response;
            }

            //  Check if the database exists in system using DatabaseService
            var validDatabases = _databaseService.GetAllDatabases();
            if (!validDatabases.Contains(registerDbDto.Db_name))
            {
                response.IsSuccess = false;
                response.Message = $"Database '{registerDbDto.Db_name}' does not exist in the system.";
                return response;
            }

            //  Check duplicate in RegisterDb table
            var exists = _unitOfWork.RegisterDbRepository.GetQuerable(x => x.Db_name == registerDbDto.Db_name && x.Project_name == registerDbDto.Project_name).Any();
            if (exists)
            {
                response.IsSuccess = false;
                response.Message = registerDbDto.Db_name +  " database already registered in the project: " + registerDbDto.Project_name;
                return response;
            }

            //  Insert record into RegisterDb table
            var entity = new RegisterDb
            {
                Project_name = registerDbDto.Project_name,
                Db_name = registerDbDto.Db_name,
                isActive = registerDbDto.isActive
            };
            _unitOfWork.RegisterDbRepository.Add(entity);
            _unitOfWork.Commit();

            response.IsSuccess = true;
            response.Message = $"Database '{registerDbDto.Db_name}' successfully registered.";
            return response;
        }

        public ValidationDTO UpdateDatabase(int userId, int id, RegisterDbDto registerDbDto)
        {
            var response = new ValidationDTO();

            if(registerDbDto == null)
            {
                response.IsSuccess = false;
                response.Message = "Db data cannot be null";
            }

            var entity = _unitOfWork.RegisterDbRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
            if(entity == null)
            {
                response.Message = "Db not found";
                return response;
            }

            entity.Project_name = (registerDbDto.Project_name ?? string.Empty).Trim();
            entity.Db_name = (registerDbDto.Db_name ?? string.Empty).Trim();
            entity.isActive = (registerDbDto.isActive);

            _unitOfWork.RegisterDbRepository.Update(entity);
            _unitOfWork.Commit();

            response.IsSuccess = true;
            response.Message = "Register DB updated succesfully";

            return response;
        }


        public RegisterDbDto PatchDatabase(int userID, int id, Dictionary<string, object> patchData)
        {
            var entity = _unitOfWork.RegisterDbRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
            if (entity == null)
            {
                return null; // controller will return 404
            }

            //  Loop through patch data
            foreach (var kvp in patchData)
            {
                var key = kvp.Key.ToLower();
                var value = kvp.Value?.ToString();

                switch (key)
                {
                    case "project_name":
                        entity.Project_name = (value ?? string.Empty).Trim();
                        break;

                    case "db_name":
                        // Validate against system databases
                        var validDatabases = _databaseService.GetAllDatabases();
                        if (!validDatabases.Contains(value))
                        {
                            throw new ArgumentException($"Database '{value}' does not exist in system.");
                        }

                        // Check duplicate inside RegisterDb table
                        var exists = _unitOfWork.RegisterDbRepository
                            .GetQuerable(x => x.Db_name == value && x.Id != id)
                            .Any();

                        if (exists)
                        {
                            throw new ArgumentException($"Database '{value}' is already registered.");
                        }

                        entity.Db_name = value.Trim();
                        break;

                    case "isactive":
                        if (bool.TryParse(value, out bool active))
                        {
                            entity.isActive = active;
                        }
                        break;
                }
            }

            // Save changes
            _unitOfWork.RegisterDbRepository.Update(entity);
            _unitOfWork.Commit();

            // Return updated DTO
            return new RegisterDbDto
            {
                Id = entity.Id,
                Project_name = entity.Project_name,
                Db_name = entity.Db_name,
                isActive = entity.isActive
            };
        }


        public ValidationDTO DeleteDatabase(int id)
        {
            var response = new ValidationDTO();

            var entity = _unitOfWork.RegisterDbRepository.GetQuerable(d => d.Id == id).FirstOrDefault();

            if (entity == null)
            {
                response.IsSuccess = false;
                response.Message = "Database not found.";
                return response;
            }

            if (entity.isActive)
            {
                response.IsSuccess = false;
                response.Message = $"Project '{entity.Project_name}' with DB '{entity.Db_name}' is still active. Deactivate before deleting.";
                return response;
            }

            // delete
            _unitOfWork.RegisterDbRepository.Delete(entity);
            _unitOfWork.Commit();

            response.IsSuccess = true;
            response.Message = $"Inactive database '{entity.Db_name}' for project '{entity.Project_name}' deleted successfully.";
            return response;
        }
        //public ValidationDTO DeleteDatabase(int id)
        //{
        //    var response = new ValidationDTO();
        //    // Find the record in RegisterDb table
        //    var entity = _unitOfWork.RegisterDbRepository
        //                            .GetQuerable(u => u.Id == id)
        //                            .FirstOrDefault();

        //    if (entity == null)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Database not found.";
        //        return response;
        //    }

        //    // Check if already inactive
        //    if (!entity.isActive)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = $"Database '{entity.Db_name}' is already inactive.";
        //        return response;
        //    }

        //    // Mark as inactive 
        //    entity.isActive = false;

        //    _unitOfWork.RegisterDbRepository.Update(entity);
        //    _unitOfWork.Commit();

        //    response.IsSuccess = true;
        //    response.Message = $"Database '{entity.Db_name}' marked as inactive successfully.";

        //    return response;
        //}



    }
}
