using Sample.Data.KamanaDB.Entities;
using Sample.Repository.Kamana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Kamana
{
    public class AppMenuService : IAppMenuService
    {
        private readonly IKamanaUnitOfWork _unitOfWork;

        public AppMenuService(IKamanaUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TblAppMenu>> GetAllAsync()
        {
            return await _unitOfWork.AppMenuRepository.GetAllAsync();
        }
    }
}
