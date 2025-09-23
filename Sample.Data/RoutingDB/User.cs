using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.RoutingDB
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
       
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
