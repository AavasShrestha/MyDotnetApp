using Sample.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Service.RegisterDbService
{
    public interface IRegisterDbService
    {
        IEnumerable<RegisterDbDto> GetAllDatabases();
        RegisterDbDto GetDatabaseById(int id);
        ValidationDTO CreateDatabase(int userID, RegisterDbDto registerDbDto);
        ValidationDTO UpdateDatabase(int userId, int id, RegisterDbDto registerDbDto);
        ValidationDTO DeleteDatabase(int id);
        RegisterDbDto PatchDatabase(int userID, int id, Dictionary<string, object> patchData);
    }
}
