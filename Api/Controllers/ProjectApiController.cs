using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Model.Models;
using Api.Service;
using Microsoft.AspNetCore.Mvc;

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
        }

        [HttpGet]
        public ActionResult<string> Get()
        {          
            var res = $" value1 + value2";
            return res;
        }

      

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ApiCreateProjectSubmitVm model)
        {
            var res = apiService.IsAddNewProjectAsync(model).Result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
