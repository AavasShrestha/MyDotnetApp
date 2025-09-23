using Sample.Data.DTO;
using Sample.Repository;
using Sample.Data.RoutingDB;

namespace Sample.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        // New CRUD methods
        public ValidationDTO CreateUser(int userId, UserDetail userDetail)
        {
            var response = new ValidationDTO();
            try
            {
                if (userDetail == null)
                {
                    response.Message = "Client data cannot be null.";
                    return response;
                }

                var userName = (userDetail.Username ?? string.Empty).Trim();
                var password = (userDetail.Password ?? string.Empty).Trim();
                var ConfirmPassword = (userDetail.ConfirmPassword ?? string.Empty).Trim();
                var Remarks = (userDetail.ConfirmPassword ?? string.Empty).Trim();
                var Phone = (userDetail.Remarks ?? string.Empty).Trim();
                var FullName = (userDetail.FullName ?? string.Empty).Trim();
                var isActive = (userDetail.IsActive);
                var Email = (userDetail.Email ?? string.Empty).Trim();
                var Gender = (userDetail.Gender ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    response.Message = "User name cannot be null or empty.";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    response.Message = "Password name cannot be null or empty.";
                    return response;
                }

                var exists = _unitOfWork.UserRepository.GetQuerable
                    (u => u.Username == userName ).Any();

                if (exists)
                {
                    response.Message = "User with the same name already exists.";
                    return response;
                }

                var entity = new User
                {
                    Username = userName,
                    Password = password,
                    ConfirmPassword = ConfirmPassword,
                    Remarks = Remarks,
                    Phone = Phone,
                    FullName = FullName,
                    IsActive = isActive,
                    Email = Email,
                    Gender = Gender
                };

                _unitOfWork.UserRepository.Add(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "User created successfully.";
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred while creating the user: {ex.Message}";
            }

            return response;
        }

        public ValidationDTO UpdateUser(int id, UserDetail userDetail)
        {
            var response = new ValidationDTO();
            try
            {
                if (userDetail == null)
                {
                    response.Message = "User data cannot be null.";
                    return response;
                }

                if (id <= 0)
                {
                    response.Message = "Invalid User id.";
                    return response;
                }

                // Find existing User from database
                var entity = _unitOfWork.UserRepository
                    .GetQuerable(u => u.Id == id)
                    .FirstOrDefault();

                if (entity == null)
                {
                    response.Message = "User not found.";
                    return response;
                }

                //  Update only allowed fields
                entity.Username = (userDetail.Username ?? string.Empty).Trim();
                entity.Password = (userDetail.Password ?? string.Empty).Trim();
                entity.ConfirmPassword = (userDetail.ConfirmPassword ?? string.Empty).Trim();
                entity.Remarks = (userDetail.Remarks ?? string.Empty).Trim();
                entity.Phone = (userDetail.Phone ?? string.Empty).Trim();
                entity.FullName = (userDetail.FullName ?? string.Empty).Trim();
                entity.IsActive = (userDetail.IsActive);
                entity.Gender = (userDetail.Gender ?? string.Empty).Trim();


                _unitOfWork.UserRepository.Update(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "User updated successfully.";

            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while updating the client. Please try again!";
            }


            return response;
        }

        public ValidationDTO DeleteUser(int id)
        {
            var response = new ValidationDTO();

            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid User id.");

                //  Find existing client
                var entity = _unitOfWork.UserRepository
                    .GetQuerable(u => u.Id == id)
                    .FirstOrDefault();

                if (entity == null)
                    throw new KeyNotFoundException("User not found.");

                //  Delete the client
                _unitOfWork.UserRepository.Delete(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "User deleted successfully.";
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "An error occurred while deleting the user. Please try again!";
            }

            return response;

        }


        public IEnumerable<UserDetail> GetAllUsers()
        {
            return _unitOfWork.UserRepository.GetQuerable()
                .Select(u => new UserDetail
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password,
                    ConfirmPassword = u.ConfirmPassword,
                    Remarks = u.Remarks,
                    Phone = u.Phone,
                    FullName = u.FullName,
                    IsActive = u.IsActive,
                    Email = u.Email,
                    Gender = u.Gender
                }).ToList();
        }

        public UserDetail GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository
                    .GetQuerable(u => u.Id == id)
         .FirstOrDefault();

            if (user == null) return null;

            return new UserDetail
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword,
                Remarks = user.Remarks,
                Phone = user.Phone,
                IsActive = user.IsActive,
                Email = user.Email,
                Gender = user.Gender
            };
        }

        // To login for user (avash and password).

        public UserDetail GetLoggedInUserDetail(string userName, string encryptedPassword)
        {
            var userData = (from u in _unitOfWork.UserRepository.GetQuerable(q => q.Username == userName && q.Password == encryptedPassword)
                            select
                    new UserDetail()
                    {
                        Id = u.Id,
                        IsActive = u.IsActive,
                        Username = u.Username,
                        FullName = u.FullName
                    }).FirstOrDefault();

            return userData;
        }

      
        public bool UserAuthentication(int userId, string password)
        {
            return _unitOfWork.UserRepository.GetQuerable(q => q.Id == userId && q.Password == password).Any();
        }


        public UserDetail PatchUser(int id, Dictionary<string, object> patchData)
        {
            var entity = _unitOfWork.UserRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
            if (entity == null)
                throw new KeyNotFoundException("User not found.");

            foreach (var key in patchData.Keys)
            {
                switch (key.ToLower())
                {
                    case "username":
                        entity.Username = patchData[key]?.ToString();
                        break;
                    case "password":
                        entity.Password = patchData[key]?.ToString();
                        break;
                    case "confirmpassword":
                        entity.ConfirmPassword = patchData[key]?.ToString();
                        break;
                    case "fullname":
                        entity.FullName = patchData[key]?.ToString();
                        break;
                    case "email":
                        entity.Email = patchData[key]?.ToString();
                        break;
                    case "phone":
                        entity.Phone = patchData[key]?.ToString();
                        break;
                    case "gender":
                        entity.Gender = patchData[key]?.ToString();
                        break;
                    case "remarks":
                        entity.Remarks = patchData[key]?.ToString();
                        break;
                    case "isactive":
                        entity.IsActive = Convert.ToBoolean(patchData[key]);
                        break;
                }
            }

            entity.ModifiedDate = DateTime.Now;

            _unitOfWork.UserRepository.Update(entity);
            _unitOfWork.Commit();

            return new UserDetail
            {
                Id = entity.Id,
                Username = entity.Username,
                Password = entity.Password,
                ConfirmPassword = entity.ConfirmPassword,
                FullName = entity.FullName,
                Email = entity.Email,
                Phone = entity.Phone,
                Gender = entity.Gender,
                Remarks = entity.Remarks,
                IsActive = entity.IsActive
            };
        }


    }
}