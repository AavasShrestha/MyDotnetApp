using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.DTO
{
    public class RegisterDbDto
    {
        public int Id { get; set; }
        public string Project_name { get; set; }
        public string Db_name { get; set; }
        public bool isActive { get; set; }
    }
}
