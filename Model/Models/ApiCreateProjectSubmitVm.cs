using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Model.Models
{
    public class ApiCreateProjectSubmitVm
    {
       
        public string Title { get; set; }
      
        public string Description { get; set; }
        public string Organization { get; set; }
      
        // public string Role { get; set; }
        public string Link { get; set; }

     
        public Guid TypeId { get; set; }
        public List<SelectListItem> Types { get; set; }
    }
}
