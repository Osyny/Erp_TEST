using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Model.Models;
using Api.Service;
using System.Net;
using ViewModelService.Models;

namespace Api.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class ProjectApiController : ControllerBase
    {
        private IApiService apiService;

        public ProjectApiController(IApiService apiService)
        {
            this.apiService = apiService;

            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };
        }

        [HttpGet]
        public List<ApiGetProjectsVm> Get()
        {
            var res = apiService.GetProjects().Result;
            return res;
        }

      
        [HttpPost]
        public string Post([FromBody] ApiProjectSubmitVm model)
        {
            var res = apiService.AddNewProjectAsync(model).Result;
            return res;
        }

       
        [HttpPut()]
        public string Put([FromBody] ApiEditProjectVm model)
        {
            var res = apiService.EditProjectAsync(model).Result;

            return res;

        }

        // DELETE api/values/5
        [HttpDelete]
        public string Delete(string userRole)
        {
            var res = apiService.DeleteAllProjectsAsync(userRole).Result;

            return res;
        }
    }
}
