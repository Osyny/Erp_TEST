using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Model.Models;
using Api.Service;
using System.Net;

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
        public ActionResult<string> Get()
        {          
            var res = $" value1 + value2";
            return res;
        }

      
        [HttpPost]
        public string Post([FromBody] ApiProjectSubmitVm model)
        {
            var res = apiService.AddNewProjectAsync(model).Result;
            return res;
        }

       
        [HttpPut()]
        public string Put([FromBody] ApiProjectSubmitVm model)
        {
            var res = apiService.EditProjectAsync(model).Result;

            return res;

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string userRole)
        {
            var res = apiService.EditProjectAsync(model).Result;

            return res;
        }
    }
}
