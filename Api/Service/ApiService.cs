using Microsoft.Extensions.Options;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModelService.Models;

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

        // Get Project
        public async Task<List<ApiGetProjectsVm>> GetProjects()
        {
            string uri = this.config.UrlProjectConection + "/Get";

           // uri += $"?prId={prId}&";

            HttpResponseMessage response = null;
            var mes = "";
            var model = new List<ApiGetProjectsVm>();
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/FileCrud"
                response = await httpClient.GetAsync(uri);

                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                var jsonQuery = JsonConvert.DeserializeObject<List<ApiGetProjectsVm>>(result);

                model = jsonQuery;

            }
            catch (Exception ex)
            {
                mes = ex.Message;

            }
            return model;
        }

        // Post
        public async Task<string> AddNewProjectAsync(ApiProjectSubmitVm model)
        {         
            string uri = this.config.UrlProjectConection + "/Post";
           // string uri = this.config.UrlProjectConection + "/CreateSubmit";

            var jsonQuery = JsonConvert.SerializeObject(model);

            var contentSend = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                response = await httpClient.PostAsync(uri, contentSend);
                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

               mes = JsonConvert.DeserializeObject<string>(result);

               
            }
            catch (Exception ex)
            {
                mes = ex.Message;
               
            }
            return mes;
        }



        //Put
        public async Task<string> EditProjectAsync(ApiEditProjectVm model)
        {
            string uri = this.config.UrlProjectConection + "/EditProject";

            var jsonQuery = JsonConvert.SerializeObject(model);

            var contentSend = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                response = await httpClient.PutAsync(uri, contentSend);
                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                mes = JsonConvert.DeserializeObject<string>(result);

            }
            catch (Exception ex)
            {
                mes = ex.Message;

            }
            return mes;
        }  
        //DeleteAll
        public async Task<string> DeleteAllProjectsAsync(string userRole)
        {
            string uri = this.config.UrlProjectConection + "/DeleteAll";

           uri += $"?userRole={userRole}&";

            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> ProgectsCrudController-> [HttpDelete]DeleteAll
                // "https://localhost:5001/Project"
               response = await httpClient.DeleteAsync(uri);
                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                mes = result;

            }
            catch (Exception ex)
            {
                mes = ex.Message;

            }
            return mes;
        }

        #region File

        // Get File
        public async Task<List<ApiDbFileVm>> GetFiles(Guid prId)
        {
            string uri = this.config.UrlFileConection + "/GetFiles";


            uri += $"?prId={prId}&";

            HttpResponseMessage response = null;
            var mes = "";
            var model = new List<ApiDbFileVm>();
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/FileCrud"
                response = await httpClient.GetAsync(uri);

                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                var jsonQuery = JsonConvert.DeserializeObject<List<ApiDbFileVm>>(result);

                model = jsonQuery;

            }
            catch (Exception ex)
            {
                mes = ex.Message;

            }
            return model;
        }

        public async Task<string> AddFileAsync(AddFileApiVm model)
        {
            string uri = this.config.UrlFileConection + "/AddFile?";


            var jsonQuery = JsonConvert.SerializeObject(model);

            var contentSend = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/FileCrud"
                response = await httpClient.PostAsync(uri, contentSend);

                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                var res = JsonConvert.DeserializeObject<string>(result);

                mes = res;
            }
            catch (Exception ex)
            {
                mes = ex.Message;

            }
            return mes;
        }

        // ProjectFile delete
        public async Task<string> DeleteFileProjectAsync(Guid fileId, Guid prId/*DeleteFileApiVm model*/)
        {
            string uri = this.config.UrlFileConection + "/DeleteFile";

            uri += $"?fileId={fileId}&prId={prId}&";
            
            HttpResponseMessage response = null;
            var mes = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/FileCrud"
                response = await httpClient.DeleteAsync(uri);
          
                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                mes = result;

            }
            catch (Exception ex)
            {
                mes = ex.Message;
                
            }
            return mes;
        }

        #endregion



        // Test
        public async Task GetRequestTestAsync()
        {
            string uri = this.config.UrlProjectConection + "/Deleter/Post";
            HttpResponseMessage response = null;
            var err = "";
            try
            {
                // --> Api -> Project-> [HttpPost]Post
                // "https://localhost:5001/Project"
                response = await httpClient.GetAsync(uri);

                var content = response.Content;
                response.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

            }
            catch (Exception ex)
            {
                err = ex.Message;
                // return Ok(new ResultStatusHelper(false) { Message = $"Service unavailable {ex.Message}" });
            }
        }

     

    
    }
}
