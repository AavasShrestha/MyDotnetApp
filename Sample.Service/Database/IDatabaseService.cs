using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Database
{
    public interface IDatabaseService
    {
        IEnumerable<string> GetAllDatabases();

    }
}
