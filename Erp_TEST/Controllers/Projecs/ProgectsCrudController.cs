using Erp_TEST.Data;
using Erp_TEST.Helper;
using Erp_TEST.Helper.DateFormaters;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModelService.Models;

namespace Erp_TEST.Controllers.Projecs
{
    public class ProgectsCrudController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AccountUser> userManager;

        public ProgectsCrudController(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<AccountUser> userManager)
        {

            this.dbContext = dbContext;

            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [Route("ProgectsCrud/Get")]
        [HttpGet]
        public async Task<List<ApiGetProjectsVm>> GetProjects()
        {
            IQueryable<Project> allProgect = dbContext.Projects
               .Include(p => p.Skills)
               .Include(p => p.Attachments)
               .Include(p => p.ProjectType);

            var modelVm = allProgect.Select(pr => new ApiGetProjectsVm()
            {

                Id = pr.Id,
                Title = pr.Title,
                Description = pr.Description,
                Organization = pr.Organization,
                End = ParseDateForProject.GetDateTimeForProgect(pr),// pr.End.HasValue && !pr.End.Value.ToString().Contains("01.01.0001") ? pr.End.Value.ToString("dd.MM.yyyy hh:mm") : "",

                Start = pr.Start.HasValue ? pr.Start.Value.ToString("dd.MM.yyyy") : "",
                Role = pr.Role,
                Link = string.IsNullOrEmpty(pr.Link) ? "" : pr.Link,
                Skills = pr.Skills.Count != 0 ? string.Join(", ", pr.Skills.Select(s => s.Name).ToArray()) : "",
                Attachments = pr.Attachments.Count != 0 ? string.Join(", ", pr.Attachments.Select(s => s.File).ToArray()) : "",
                ProjectType = pr.ProjectType.NameType,
                Create = pr.Created.ToString("dd.MM.yyyy"),
                Update = pr.Updated.ToString("dd.MM.yyyy")

            }).ToList();

            return modelVm;
        }
        [Route("ProgectsCrud/Post")]
        [HttpPost]
        public async Task<string> CreateSubmit([FromBody] CreateProjectSubmitVm model)
        {
            //var roles = this.roleManager.Roles.ToList();
            var rolesUser = new Roles(this.roleManager, this.userManager);


            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            ////var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == userName);
            //var addedRoles = roles.Except(roles).ToList();

            string userRole = "";
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == model.UserName);

                if (curentuser != null)
                {
                    userRole = rolesUser.GetRole(curentuser.AccountUser);
                }              

                var newProduct = new Project()
                {
                    Id = Guid.NewGuid(),
                };

                if (!string.IsNullOrEmpty(model.Title) && !string.IsNullOrEmpty(model.Description)
                    && model.TypeId != default)
                {
                    var type = this.dbContext.Types.FirstOrDefault(t => t.Id == model.TypeId);

                    newProduct.Title = model.Title;

                    newProduct.Description = model.Description;
                    newProduct.Organization = model.Organization;
                    newProduct.Role = userRole;
                    newProduct.Link = model.Link;

                    newProduct.ProjectTypeId = type.Id;

                    newProduct.Created = DateTime.Now;
                    newProduct.Updated = DateTime.Now;



                    dbContext.Projects.Add(newProduct);
                    await dbContext.SaveChangesAsync();


                    return "The project was successfully created";
                }
                else
                {
                    return "Title, Description, Role, Type - must be filled";
                }
                
            }
            return "UserName - must be filled";

        }
        // Put
        public string EditProject([FromBody]ApiEditProjectVm model)
        {
            var mes = "";
            var prAll = dbContext.Projects
                .Include(p => p.Attachments)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == model.Id);

            var startRes = DateParsers.ddMMyyyy(model.Start);
            if (startRes.IsValid)
            {
                DateTime dateStart = new DateTime(startRes.Value.Year,
                  startRes.Value.Month,
                  startRes.Value.Day);

                updatePr.Start = dateStart;


            }
            

            var endRes = DateParsers.ddMMyyyy(model.End);
            var endtimeRes = DateParsers.HHmm(model.EndTime);
            DateTime dateEnd = default;
            if (endRes.IsValid)
            {
                dateEnd = new DateTime(startRes.Value.Year,
                  startRes.Value.Month,
                  startRes.Value.Day);

            }        

            if (endtimeRes.IsValid)
            {
                dateEnd = dateEnd.AddHours(endtimeRes.Value.Hour);
                dateEnd = dateEnd.AddMinutes(endtimeRes.Value.Minute);

            }
        
            if (endRes.IsValid || endtimeRes.IsValid)
            {
                updatePr.End = dateEnd;
            }
            var type = this.dbContext.Types.FirstOrDefault(t => t.Id == model.TypeId);
           
            
            updatePr.Title = model.Title;
            updatePr.Description = model.Description;
            updatePr.Organization = model.Organization;
            updatePr.Role =  model.Role;
            updatePr.Link = model.Link;

            updatePr.ProjectTypeId = type.Id;
            updatePr.Updated = DateTime.Now;

            dbContext.Update(updatePr);
            dbContext.SaveChanges();
            mes = "Project змінено";

            return mes;
        }

        //[Route("ProgectsCrud/Delete")]
        [HttpDelete]
        public string DeleteAll(string userRole)
        {
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();

            string mess = "";
            if(userRole == "Admin")
            {
                foreach (var pr in prAll)
                {
                    dbContext.Projects.Remove(pr);
                }
                try
                {
                    dbContext.SaveChanges();
                    mess = "Всі проекти видалено успішно";
                }
                catch (Exception ex)
                {
                    mess = ex.Message;                    
                }
            }
            else
            {
                mess = "Не має прав доступу для видалення!!!";
            }           

            return mess;
        }
    }
}
