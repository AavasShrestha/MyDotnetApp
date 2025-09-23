using Sample.Data.DTO;
using Sample.Repository;
using Sample.Service.Cache;
using SWCommon.Static;

namespace Sample.Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IUserService _userService;

        public LoginService(IUnitOfWork unitOfWork, IUserPreferenceService userPreferenceService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userPreferenceService = userPreferenceService;
            _userService = userService;
        }

        public (bool isLoginSuccess, UserDetail userDetail) GetLoggedInUserDetail(string userName, string password)
        {
            var encryptedPassword = StringEncryption.Encrypt(password);
            var data = _userService.GetLoggedInUserDetail(userName, encryptedPassword);

            if (data != null)
            {
                return data.IsActive ? (true, data) : (false, null);
            }
            else
            {
                return (false, null);
            }
        }
    }
}