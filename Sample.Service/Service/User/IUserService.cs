using Sample.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service
{
    public interface IUserService
    {
        UserDetail GetLoggedInUserDetail(string userName, string encryptedPassword);
        bool UserAuthentication(int userId, string password);



        // New CRUD methods:
        ValidationDTO CreateUser(int userId, UserDetail userDetail);
        ValidationDTO UpdateUser(int id, UserDetail userDetail);
        ValidationDTO DeleteUser(int id);
        IEnumerable<UserDetail> GetAllUsers();
        UserDetail GetUserById(int id);

        UserDetail PatchUser(int id, Dictionary<string, object> patchData);
    }
}