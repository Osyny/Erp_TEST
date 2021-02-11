using Erp_TEST.Data;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AccountUser> userManager;

        public HomeController(ILogger<HomeController> logger,
             RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            UserManager<AccountUser> userManager)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var roles = this.roleManager.Roles.ToList();
            if (roles.Count == 0 && !roles.Any())
            {

                IdentityResult resultAdmin = await this.roleManager.CreateAsync(new IdentityRole("Admin"));
                IdentityResult resultUser = await this.roleManager.CreateAsync(new IdentityRole("User"));

                if (!resultAdmin.Succeeded)
                {
                    return Content($"Роль -'Admin' не створена");
                }
                if (!resultUser.Succeeded)
                {
                    return Content($"Роль -'User' не створена");
                }
            }


            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            var admin = allUsers.FirstOrDefault(u => u.AccountUser.Email == "admin@gmail.com");
            if (admin == null)
            {
                AccountUser accountUser = new AccountUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
                var res = await this.userManager.CreateAsync(accountUser, "Q1234_qaz");
                await this.userManager.AddToRoleAsync(accountUser, "Admin");

                string emailConfirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(accountUser);
                var confirmResult = await this.userManager.ConfirmEmailAsync(accountUser, emailConfirmationToken);

                if (res.Succeeded)
                {
                    var newUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        AccountUser = accountUser,
                        DateRegister = DateTime.Now

                    };
                    dbContext.ListUsers.Add(newUser);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    return Content("ERROR");
                }

            }
            var allType = dbContext.Types.ToList();
            if(allType.Count==0)
            {
                dbContext.Types.AddRange(
                    new ProjectType()
                    {
                        Id = Guid.NewGuid(),
                        NameType = "Work",
                    } ,
                    new ProjectType()
                    {
                        Id = Guid.NewGuid(),
                        NameType = "Book",
                    },
                    new ProjectType()
                    {
                        Id = Guid.NewGuid(),
                        NameType = "Course",
                    },
                    new ProjectType()
                    {
                        Id = Guid.NewGuid(),
                        NameType = "Blog",
                    },
                    new ProjectType()
                    {
                        Id = Guid.NewGuid(),
                        NameType = "Other",
                    }
                 );
                dbContext.SaveChanges();
            }
         

            return RedirectToAction(actionName: "Index", controllerName: "Project");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
