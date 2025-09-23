using Microsoft.AspNetCore.Http;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Service.LogoService
{
    public interface ILogoService
    {
        ValidationDTO UploadLogo(IFormFile file);
        ValidationDTO UpdateLogo(int id, IFormFile file);
        IEnumerable<Logo> GetAllLogos();
        Logo GetLogoById(int id);
        ValidationDTO DeleteLogo(int id);
        
    }
}
