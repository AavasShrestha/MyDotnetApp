using Sample.Data.KamanaDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Kamana
{
    public interface IAppMenuService
    {
        Task<IEnumerable<TblAppMenu>> GetAllAsync();
    }
}
