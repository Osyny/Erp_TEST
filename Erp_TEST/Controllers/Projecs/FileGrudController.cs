using Erp_TEST.Data;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModelService.Models;

namespace Erp_TEST.Controllers.Projecs
{
    public class FileGrudController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AccountUser> userManager;
        private readonly IHostingEnvironment environment;

        public FileGrudController(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<AccountUser> userManager,
             IHostingEnvironment environment)
        {

            this.dbContext = dbContext;

            this.roleManager = roleManager;
            this.userManager = userManager;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<List<ApiDbFileVm>> GetFiles( Guid  prId)
        {
            var mess = "";
            var pr = dbContext.Projects
              .Include(p => p.Attachments)
              .FirstOrDefault(f => f.Id == prId);

            var files = pr.Attachments.ToList();
            var model = files.Select(f => new ApiDbFileVm()
            {
                FileId = f.Id,
                File = f.File,
                Name = f.FileName,
                DateCreate = f.DateCreate.ToString("dd.MM.yyyy")
            }).ToList();


            return model;
        }

            [HttpPost]
        public async Task<string> AddFileAsync([FromBody]AddFileApiVm model)
        {
            var prAll = dbContext.Projects
              .Include(p => p.Attachments)
              .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == model.ProjId);

            var mess = "";
            if (model.File != null)
            {
                string name = model.File.FileName;
                string path = $"/files/{name}";
                string serverPath = $"{this.environment.WebRootPath}{path}";
                FileStream fs = new FileStream(serverPath, FileMode.Create,
                    FileAccess.Write);
                await model.File.CopyToAsync(fs);
                fs.Close();

                var newFile = new DbFile()
                {
                    Id = Guid.NewGuid(),
                    File = path,
                    FileName = name,
                    DateCreate = DateTime.Now
                };

                dbContext.DbFiles.Add(newFile);
                dbContext.SaveChanges();


                updatePr.Attachments.Add(newFile);
                updatePr.Updated = DateTime.Now;
                dbContext.Update(updatePr);

                await dbContext.SaveChangesAsync();
                mess = "Файл завaнтажено!!!";
            }
            else
            {
                mess = "Файл не вибрано!!!";
            }
            return mess;
        }


        [HttpDelete]
        public string DeleteFile(Guid fileId, Guid prId)
        {
            
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == prId);

            var fileAll = dbContext.DbFiles.ToList();
            var fileFound = fileAll.FirstOrDefault(f => f.Id == fileId);
            if(fileFound == null && prAll == null)
            {
                return "File або project не знайдено!!!";
            }

            updatePr.Attachments.Remove(fileFound);
            dbContext.DbFiles.Remove(fileFound);
            dbContext.SaveChanges();

            return "File видалено успішно!";
        }
    }
}
