using Microsoft.Extensions.Options;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;


        private readonly UrlConection config;

        public ApiService(HttpClient httpClient, IOptions<UrlConection> config)
        {
            this.httpClient = httpClient;

            this.config = config.Value;
        }

        public async Task<bool> IsAddNewProjectAsync(ApiCreateProjectSubmitVm model)
        {

            string uri = this.config.AddNewProject + "/Post";

            //uri += $"userDataQuery={userDataQuery}&";

            var jsonQuery = JsonConvert.SerializeObject(model);

            var contentSend = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                response = await httpClient.PostAsync(uri, contentSend);
                var i = response;

            }
            catch (Exception ex)
            {
                var hh = ex.Message;
                // return Ok(new ResultStatusHelper(false) { Message = $"Service unavailable {ex.Message}" });
            }
            return true;
        }
    }
}
