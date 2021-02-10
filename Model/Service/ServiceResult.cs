using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelService.Service
{
    public class ServiceResult<TModel> where TModel : class
    {
        public ServiceResult()
        {

        }
        public ServiceResult(Task<HttpResponseMessage> response)
        {
            try
            {
                var content = response.Result.Content;
                response.Result.EnsureSuccessStatusCode();
                string result = content.ReadAsStringAsync().Result;

                this.Result = result;
                //try
                //{
                //    var testRes = JsonConvert.DeserializeObject<TModel>(result);
                //}
                //catch (Exception e)
                //{
                //    // брекйпоинт для проверки
                //    var mess = e.Message;
                //    //Console.WriteLine(e);
                //}


                //this.ResultParsed = testRes;
                this.IsSucess = true;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                this.IsSucess = false;
            }
        }

        public string Error { get; set; }
        public string Result { get; set; }

        public bool IsSucess { get; set; }
        //public TModel ResultParsed { get; set; }

        public TModel GetParsed()
        {
            return JsonConvert.DeserializeObject<TModel>(this.Result);
        }
    }

    public class ServiceResultResponseJSON
    {
        public string error { get; set; }
        public string result { get; set; }

        public bool isSucess { get; set; }
    }
}
