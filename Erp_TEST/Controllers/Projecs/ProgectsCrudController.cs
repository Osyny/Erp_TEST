using Erp_TEST.Data;
using Erp_TEST.Helper;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    return "Title, Description, Type - must be filled";
                }
                
            }
            return "UserName - must be filled";

        }
    }
}
