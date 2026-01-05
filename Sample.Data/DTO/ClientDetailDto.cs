using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string? Owner { get; set; }

        public string? Address { get; set; }
        
        public string? Primary_phone { get; set; }
        public string? Secondary_phone { get; set; }

        public string? Primary_email { get; set; }
        public string? Secondary_email { get; set; }
        public bool SMS_service { get; set; } = false;
        public bool ApprovalSystem { get; set; } = false;
        public bool CollectionApp { get; set; } = false;

        public bool CID { get; set; }
        public string? db_username { get; set; }
        public string? db_pwd { get; set; }
        public string? server_name { get; set; }


    }
}
