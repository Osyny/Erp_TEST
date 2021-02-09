using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.DbModel
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
    }
}
