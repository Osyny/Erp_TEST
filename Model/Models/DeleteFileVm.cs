using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModelService.Models
{
    public class DeleteFileVm
    {
        public Guid FileId { get; set; }
        public Guid ProjId { get; set; }

    }
    public class AddFileApiVm
    {

        public Guid ProjId { get; set; }
        public IFormFile File { get; set; }


    }

    public class ApiDbFilesVm
    {
        public List<ApiDbFileVm> Files { get; set; }
    }
    public class ApiDbFileVm
    {
        public Guid FileId { get; set; }
        public string File { get; set; }
        public string Name { get; set; }
        public string DateCreate { get; set; }
    }
}
