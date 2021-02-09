using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.ViewModel.Projects
{
    public class UploadFileSubmitVm
    {

        public Guid Id { get; set; }
        public string File { get; set; }
    }
    public class EditProjectVm
    {

        public Guid Id { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }
        public string Organization { get; set; }
        public string End { get; set; }
        public string Start { get; set; }

        public string Role { get; set; }
        public string Link { get; set; }

        public string Skills { get; set; }


        public List<FileVm> AttachmentVm { get; set; }
        public List<SkillVm> SkillsVm { get; set; }

        public string Create { get; set; }
        public string Update { get; set; }

        public string User { get; set; }


        public string ProjectType { get; set; }
        public Guid TypeId { get; set; }
        public List<SelectListItem> Types { get; set; }
    }
    public class FileVm
    {
        public Guid Id { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
    }
    public class SkillVm
    {
        public Guid Id { get; set; }
        public string SkillName { get; set; }
       
    }

     public class EditProjectSubmitVm
    {

        public Guid Id { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }
        public string Organization { get; set; }
        public string End { get; set; }
        public string Start { get; set; }

        public string Role { get; set; }
        public string Link { get; set; }

        public string Skills { get; set; }


        public string Attachments { get; set; }
        
        public string Create { get; set; }
        public string Update { get; set; }

        public string User { get; set; }


        public string ProjectType { get; set; }
        public Guid TypeId { get; set; }
        public List<SelectListItem> Types { get; set; }
    }
}
