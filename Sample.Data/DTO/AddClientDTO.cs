using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.DTO
{
    public class AddClientDTO
    {
        public string? client_name { get; set; } 
        public string? db_name { get; set; }
        public IFormFile logo { get; set; }
    }
}
