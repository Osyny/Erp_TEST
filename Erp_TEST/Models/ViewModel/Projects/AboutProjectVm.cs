using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.ViewModel.Projects
{
    public class AboutProjectVm
    {

        public Guid Id { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }
        public string Organization { get; set; }
        public string End { get; set; }
        public string EndTime { get; set; }
        public string Start { get; set; }

        public string Role { get; set; }
        public string Link { get; set; }

        public string Skills { get; set; }


        public List<AboutFileVm> AttachmentVm { get; set; }
        public List<AboutSkillVm> SkillsVm { get; set; }

        public string Create { get; set; }
        public string Update { get; set; }

        public string User { get; set; }


        public string ProjectType { get; set; }
       
    }
    public class AboutFileVm
    {
        public Guid Id { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public string Data { get; set; }
    }
    public class AboutSkillVm
    {
        public Guid Id { get; set; }
        public string SkillName { get; set; }
        public string Data { get; set; }

    }
}
