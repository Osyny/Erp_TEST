using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Model.Models
{
    public class ApiProjectSubmitVm
    {
       
        public string Title { get; set; }
      
        public string Description { get; set; }
        public string Organization { get; set; }
      
        public string Role { get; set; }
        public string Link { get; set; }

     
        public Guid TypeId { get; set; }
        public List<SelectListItem> Types { get; set; }

        public string UserName { get; set; }
    }

    public class DeleteFileApiVm
    {
        public Guid FileId { get; set; }
        public Guid PrId { get; set; }
    }

    public class ApiEditProjectVm
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

        //  public string Skills { get; set; }


        //public List<FileVm> AttachmentVm { get; set; }
        //public List<SkillVm> SkillsVm { get; set; }

        //  public string Create { get; set; }
        //public string Update { get; set; }

        public string User { get; set; }


        //  public string ProjectType { get; set; }
        //  public string ProjectType { get; set; }
        public Guid? TypeId { get; set; }
        //  public List<SelectListItem> Types { get; set; }
    }


    public class EditFileApiVm
    {
        public Guid FileId { get; set; }
        public string NameFile { get; set; }
       
    }
}
