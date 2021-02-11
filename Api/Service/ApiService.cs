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

       
        public async Task<string> AddNewProjectAsync(ApiCreateProjectSubmitVm model)
        {

            string uri = this.config.AddNewProject + "/Post";

            var jsonQuery = JsonConvert.SerializeObject(model);

            var contentSend = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                var response1 = await httpClient.PostAsync(uri, contentSend);
                mes = response.ToString();
                
            }
            catch (Exception ex)
            {
                mes = ex.Message;
               
            }
            return mes;
        }

        public async Task GetRequestTestAsync()
        {
            string uri = this.config.AddNewProject + "/Deleter/Post";
            HttpResponseMessage response = null;
            var err = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                response = await httpClient.GetAsync(uri);
                var i = response;

            }
            catch (Exception ex)
            {
                err = ex.Message;
                // return Ok(new ResultStatusHelper(false) { Message = $"Service unavailable {ex.Message}" });
            }
        }

        public async Task DeleteFileProjectAsync(string jsonModel)
        {
            string uri = this.config.AddNewProject + "/Deleter/Post?";

            // var jsonQuery = JsonConvert.SerializeObject(model);
            uri += $"jsonModel={jsonModel}&";
            //var contentSend = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            var err = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/ProgectsCrud"
                response = await httpClient.GetAsync(uri);
                var i = response;

            }
            catch (Exception ex)
            {
                err = ex.Message;
                // return Ok(new ResultStatusHelper(false) { Message = $"Service unavailable {ex.Message}" });
            }
        }

      
    }
}
