using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.KamanaDB.Entities
{
    [Table("Tbl_AppMenus")]
    public class TblAppMenu
    {
        [Key]
        public int ID { get; set; }   
        public Guid? MenuId { get; set; }  

        public string? MenuName { get; set; }

        public string? NepaliName { get; set; }

        public string? MenuDescription { get; set; }

        public bool? IsActive { get; set; }  
        public string? MenuGroup { get; set; }

        public int? MenuGroupID { get; set; }

        public bool? BackDateEntryAllowed { get; set; } 

    }
}
