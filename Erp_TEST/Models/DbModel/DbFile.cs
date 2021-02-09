using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.DbModel
{
    public class DbFile
    {
        public Guid Id { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public DateTime DateCreate { get; set; }

    }
}
