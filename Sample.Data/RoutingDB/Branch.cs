using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.RoutingDB
{
    [Table("tblBranch")]
    public class Branch
    {
        [Key]
        public int SN { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string NameEng { get; set; }
        public bool? IsTimeEnforced { get; set; }
        public TimeSpan? OfficeOpenTime { get; set; }
        public TimeSpan? OfficeCloseTime { get; set; }
    }
}
