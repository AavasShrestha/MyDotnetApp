using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.DTO
{
    public class ClientDetailDto
    {
        public int client_id { get; set; }               // used on GET/PUT
        public string client_name { get; set; } 
        public string db_name { get; set; } 
        public int created_by { get; set; }
        public int modified_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string? Logo { get; set; }

    }
}
