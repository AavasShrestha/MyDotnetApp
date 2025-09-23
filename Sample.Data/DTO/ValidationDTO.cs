using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.DTO
{
    public class ValidationDTO
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }

    }
}
