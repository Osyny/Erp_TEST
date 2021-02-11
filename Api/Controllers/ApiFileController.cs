using Api.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
        public ActionResult<string> Get()
        {
            apiService.GetRequestTestAsync();
            return "TEST GET";
        }

        // POST api/values
        [HttpPost]
        public void Post()
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void DeleteAsync(Guid fileId, Guid prId)
        {
            var model = new DeleteFileApiVm()
            {
                FileId = fileId,
                PrId = prId

            };
            var jsonModel = JsonConvert.SerializeObject(model);

              apiService.DeleteFileProjectAsync(jsonModel);
        }
    }
}
