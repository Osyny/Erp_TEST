using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModelService.Models
{
    public class ApiGetProjectsVm
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


            public string ProjectType { get; set; }
            public string Create { get; set; }
            public string Update { get; set; }

            public string User { get; set; }

        
    }
}
