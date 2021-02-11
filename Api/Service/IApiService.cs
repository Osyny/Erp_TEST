using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service
{
    public interface IApiService
    {
        Task<string> AddNewProjectAsync(ApiProjectSubmitVm model);
        Task<string> EditProjectAsync(ApiProjectSubmitVm model);
        Task<string> EditProjectAsync(ApiProjectSubmitVm model);

        Task DeleteFileProjectAsync(string jsonModel);

        Task GetRequestTestAsync();
    }
}
