using Sample.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Service.Client
{
    public interface IClientDetailService
    {
        ValidationDTO CreateClient(int userID, AddClientDTO clientDetailDto);
        ValidationDTO UpdateClient(int userID, int id, ClientDetailDto clientDetailDto);
        ValidationDTO DeleteClient(int id);
        IEnumerable<ClientDetailDto> GetAllClients();
        ClientDetailDto GetClientById(int id);
        ClientDetailDto PatchClient(int userID, int id, Dictionary<string, object> patchData);
    }
}
