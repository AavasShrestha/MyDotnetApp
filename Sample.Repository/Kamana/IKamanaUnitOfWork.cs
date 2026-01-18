using Sample.Data.KamanaDB.Entities;
using Sample.Data.KamanaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Repository.Kamana
{
    public interface IKamanaUnitOfWork
    {
        IRepository<TblAppMenu> AppMenuRepository { get; }
        KamanaDbContext DbContext { get; }

        bool Commit();
    }
}
