using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service
{
    public interface IApiService
    {
        Task<bool> IsAddNewProjectAsync(ApiCreateProjectSubmitVm model);
    }
}
