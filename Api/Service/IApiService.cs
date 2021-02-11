using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModelService.Models;

namespace Api.Service
{
    public interface IApiService
    {
        Task<List<ApiGetProjectsVm>> GetProjects();
        Task<string> AddNewProjectAsync(ApiProjectSubmitVm model);
        Task<string> EditProjectAsync(ApiEditProjectVm model);
        Task<string> DeleteAllProjectsAsync(string userName);

        // File
        Task<string> DeleteFileProjectAsync(Guid fileId, Guid prId);
        Task<string> AddFileAsync(AddFileApiVm model);
        Task<List<ApiDbFileVm>> GetFiles(Guid prId);

        Task GetRequestTestAsync();
    }
}
