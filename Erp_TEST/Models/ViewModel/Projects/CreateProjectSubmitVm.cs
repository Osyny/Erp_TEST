using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Erp_TEST.Models.ViewModel.Projects
{
    public class CreateProjectSubmitVm
    {
        [Required(ErrorMessage = "Поле повинно бути заповнене")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле повинно бути заповнене")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string Organization { get; set; }
        //public DateTime End { get; set; }
        // public string Role { get; set; }
        public string Link { get; set; }

        [Required(ErrorMessage = "Поле 'Type' повинно бути вибрано")]
        [Display(Name = "Type")]
        public Guid TypeId { get; set; }
        public List<SelectListItem> Types { get; set; }

        public string UserName { get; set; }
    }
}
