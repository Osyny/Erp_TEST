using Api.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModelService.Models;

namespace Api.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class ApiFileController : ControllerBase
    {
        private IApiService apiService;

        public ApiFileController(IApiService apiService)
        {
            this.apiService = apiService;
        }

        [HttpGet]
        public ActionResult<List<ApiDbFileVm>> Get(Guid prId)
        {
            var res = apiService.GetFiles(prId).Result;
            return res;
        }

        // POST api/values
        [HttpPost]
        public string Post(AddFileApiVm model)
        {
            var res = apiService.AddFileAsync(model).Result;
            return res;
        }

        // PUT api/values/5
        [HttpPut]
        public void Put([FromBody] EditFileApiVm model)
        {
            //var res = apiService.EditProjectAsync(model).Result;
            //return res;
        }

        // DELETE api/values/5
        [HttpDelete]
        public string Delete(Guid fileId, Guid prId)
        {

            var mes = apiService.DeleteFileProjectAsync(fileId, prId).Result;
            return mes;
        }
    }
}
