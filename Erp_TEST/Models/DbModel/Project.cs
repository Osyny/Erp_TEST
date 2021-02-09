using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.DbModel
{
    public class Project
    {

        public Guid Id { get; set; }
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        public string Organization { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public string Role { get; set; }
        public string Link { get; set; }

        public List<Skill> Skills { get; set; }


        public List<DbFile> Attachments { get; set; } = new List<DbFile>();


        public ProjectType ProjectType { get; set; }
        public Guid ProjectTypeId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
