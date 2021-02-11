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

        [HttpPost]
        public async Task<string> CreateSubmit([FromBody] CreateProjectSubmitVm model)
        {
            //var roles = this.roleManager.Roles.ToList();
            var rolesUser = new Roles(this.roleManager, this.userManager);


            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            ////var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == userName);
            //var addedRoles = roles.Except(roles).ToList();

            string userRole = "";
            if (string.IsNullOrEmpty(model.UserName))
            {
                var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == model.UserName);

                if (curentuser != null)
                {                  

                    var roleUser = rolesUser.GetRole(curentuser.AccountUser);
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
        public string EditProject(ApiEditProjectVm model)
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
            else if(!string.IsNullOrEmpty(model.Start))
            {
                mes = "Поле 'Start date' не вірний формат";
                return mes;
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
            else if (!string.IsNullOrEmpty(model.End))
            {
                mes = "Поле 'End date' не вірний формат";
                return mes;
            }

            if (endtimeRes.IsValid)
            {
                dateEnd = dateEnd.AddHours(endtimeRes.Value.Hour);
                dateEnd = dateEnd.AddMinutes(endtimeRes.Value.Minute);


            }
            else if (!string.IsNullOrEmpty(model.EndTime))
            {
                mes = "Поле 'EndTime' не вірний формат";
                return mes;
            }

            if (endRes.IsValid || endtimeRes.IsValid)
            {
                updatePr.End = dateEnd;
            }
            var type = this.dbContext.Types.FirstOrDefault(t => t.Id == model.TypeId);

            updatePr.Title = model.Title;
            updatePr.Description = model.Description;
            updatePr.Organization = model.Organization;
            updatePr.Link = model.Link;


            updatePr.ProjectTypeId = type.Id;
            updatePr.Updated = DateTime.Now;

            dbContext.Update(updatePr);
            dbContext.SaveChanges();
            mes = "Project змінено";

            return mes;
        }

        public string DeleteAll(string userRole)
        {
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();

            string mess = "";

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
                return mess;
            }            

            return mess;
        }
    }
}
