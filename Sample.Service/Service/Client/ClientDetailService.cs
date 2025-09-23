using Sample.Data.DTO;
using Sample.Repository;
using Sample.Data.RoutingDB;
using Microsoft.Extensions.Configuration;
using Sample.Service.Database;
using Azure.Core;
using Microsoft.AspNetCore.Http;


namespace Sample.Service.Service.Client

{
    public class ClientDetailService : IClientDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientDetailService(IUnitOfWork unitOfWork, IConfiguration configuration, IDatabaseService databaseService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _databaseService = databaseService;
            _httpContextAccessor = httpContextAccessor;
        }
        public ValidationDTO CreateClient(int userID, AddClientDTO clientDetailDto)
        {
            var response = new ValidationDTO();

            try
            {
                if (clientDetailDto == null)
                {
                    response.Message = "Client data cannot be null.";
                    return response;
                }

                var clientName = (clientDetailDto.client_name ?? string.Empty).Trim();
                var dbName = (clientDetailDto.db_name ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(clientName))
                {
                    response.Message = "Client name cannot be null or empty.";
                    return response;
                } 

                if (string.IsNullOrWhiteSpace(dbName))
                {
                    response.Message = "Database name cannot be null or empty.";
                    return response;
                }
                    

                var exists = _unitOfWork.ClientDetailsRepository.GetQuerable(c => c.client_name == clientName && c.db_name == dbName).Any();

                if (exists)
                {
                    response.Message = "Client with the same name and database already exists.";
                    return response;
                }

                var allowedDatabases = _databaseService.GetAllDatabases();

                //  Validate database exists**
                if (!allowedDatabases.Contains(dbName))
                {
                    response.Message = $"The database '{dbName}' does not exist or is not allowed.";
                    return response;
                }

                //Logo Image Handling start
                if (clientDetailDto.logo == null || clientDetailDto.logo.Length == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid file";
                    return response;
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "logos", clientDetailDto.db_name);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var newFileName = $"{clientDetailDto.logo.FileName}";
                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    clientDetailDto.logo.CopyTo(stream);
                }

                //var baseUrl = GetBaseUrl();
                
                var logoName = clientDetailDto.logo.FileName;

                // Combine virtual URL path for public access
                var logoUrl = Path.Combine("uploads", "logos", logoName).Replace("\\", "/");

                //Logo Image Handling end


                var entity = new tblClientDetails
                {
                    client_name = clientName,
                    db_name = dbName,
                    created_date = DateTime.UtcNow,
                    created_by = userID,
                    modified_date = DateTime.UtcNow,
                    modified_by = userID,
                    Logo = logoUrl
                };

                _unitOfWork.ClientDetailsRepository.Add(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "Client created successfully.";
            }

            catch (Exception ex) {
                response.Message = "An error occurred while creating the client. Please try again!";
            }

            return response;
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            return request != null
                ? $"{request.Scheme}://{request.Host}{request.PathBase}"
                : string.Empty;
        }

        public ValidationDTO UpdateClient(int userID, int id, ClientDetailDto clientDetailDto)
        {
            var response = new ValidationDTO();

            try
            {
                if (clientDetailDto == null)
                {
                    response.Message = "Client data cannot be null.";
                    return response;
                }

                if (id <= 0)
                {
                    response.Message = "Invalid client id.";
                    return response;
                }

                // Find existing client from database
                var entity = _unitOfWork.ClientDetailsRepository
                    .GetQuerable(c => c.client_id == id)
                    .FirstOrDefault();

                if (entity == null)
                {
                    response.Message = "Client not found.";
                    return response;
                }

                //  Update only allowed fields
                entity.client_name = (clientDetailDto.client_name ?? string.Empty).Trim();
                entity.db_name = (clientDetailDto.db_name ?? string.Empty).Trim();

                //Set audit fields
                entity.modified_date = DateTime.UtcNow;
                entity.modified_by = userID;

                _unitOfWork.ClientDetailsRepository.Update(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "Client updated successfully.";

            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while updating the client. Please try again!";
            }

          
            return response;
        }
        public ValidationDTO DeleteClient(int id)
        {
            var response = new ValidationDTO();
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid client id.");

                //  Find existing client
                var entity = _unitOfWork.ClientDetailsRepository
                    .GetQuerable(c => c.client_id == id)
                    .FirstOrDefault();

                if (entity == null)
                    throw new KeyNotFoundException("Client not found.");

                //  Delete the client
                _unitOfWork.ClientDetailsRepository.Delete(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "Client deleted successfully.";
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "An error occurred while deleting the client. Please try again!";
            }
          
            return response; 
        }

        public IEnumerable<ClientDetailDto> GetAllClients()
        {
            //  Fetch all client entities from the database
            var clients = _unitOfWork.ClientDetailsRepository.GetQuerable().ToList();

            var baseUrl = GetBaseUrl();

            

            //  Map entities to DTOs
            var clientDtos = clients.Select(c => new ClientDetailDto
            {
                client_id = c.client_id,
                client_name = c.client_name,
                db_name = c.db_name,
                created_date = c.created_date,
                created_by = c.created_by,
                modified_date = c.modified_date,
                modified_by = c.modified_by,
                Logo = String.IsNullOrEmpty(c.Logo) ? null : Path.Combine(baseUrl, c.Logo)
            }).ToList();

            //  Return the list of DTOs
            return clientDtos;
        }

        public ClientDetailDto GetClientById(int id)
        {
            //  Validate the id
            if (id <= 0)
                throw new ArgumentException("Invalid client id.");

            //  Fetch the client entity from the database
            var client = _unitOfWork.ClientDetailsRepository.GetQuerable(c => c.client_id == id).FirstOrDefault();

            //  If no client found, return null
            if (client == null)
                return null;

            //  Map entity to DTO
            var clientDto = new ClientDetailDto
            {
                client_id = client.client_id,
                client_name = client.client_name,
                db_name = client.db_name,
                created_date = client.created_date,
                created_by = client.created_by,
                modified_date = client.modified_date,
                modified_by = client.modified_by
            };

            //  Return the DTO
            return clientDto;
        }

       

        // Helper to map entity to DTO
        private ClientDetailDto ToDto(tblClientDetails entity)
        {
            return new()
            {
                client_id = entity.client_id,
                client_name = entity.client_name,
                db_name = entity.db_name,
                created_by = entity.created_by,
                modified_by = entity.modified_by,
                created_date = entity.created_date,
                modified_date = entity.modified_date
            };
        }

        public ClientDetailDto PatchClient(int userID, int id, Dictionary<string, object> updates)
        {
            if (updates == null || updates.Count == 0)
                throw new ArgumentException("No fields provided for update.");

            var entity = _unitOfWork.ClientDetailsRepository.GetQuerable(c => c.client_id == id).FirstOrDefault()
                         ?? throw new KeyNotFoundException("Client not found.");

            foreach (var kvp in updates)
            {
                switch (kvp.Key.ToLower())
                {
                    case "client_name":
                        entity.client_name = kvp.Value?.ToString()?.Trim();
                        break;
                    case "db_name":
                        entity.db_name = kvp.Value?.ToString()?.Trim();
                        break;
                    case "modified_by":
                        if (kvp.Value != null && int.TryParse(kvp.Value.ToString(), out int modBy))
                            entity.modified_by = modBy;
                        break;
                }
            }

            entity.modified_date = DateTime.UtcNow;
            _unitOfWork.ClientDetailsRepository.Update(entity);
            _unitOfWork.Commit();

            return ToDto(entity);
        }

       
    }
}
