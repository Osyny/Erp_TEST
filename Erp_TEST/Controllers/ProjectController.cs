﻿using Erp_TEST.Data;
using Erp_TEST.Helper.DateFormaters;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Controllers
{
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AccountUser> userManager;
        private readonly IHostingEnvironment environment;

        public ProjectController(ApplicationDbContext dbContext, IHostingEnvironment environment,
            RoleManager<IdentityRole> roleManager,
              UserManager<AccountUser> userManager)
        {
            this.dbContext = dbContext;

            this.roleManager = roleManager;
            this.userManager = userManager;

            this.environment = environment;
        }

        public ActionResult Index(string titleOrganizationType, int currentPage = 1)
        {
            var roles = this.roleManager.Roles.ToList();

            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            var userName = HttpContext.User.Identity.Name;
            string userRole = "";
            if (!string.IsNullOrEmpty(userName))
            {
                var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == userName);

                userRole = getRole(curentuser.AccountUser);
            }


            //  var skills = 
            var allProgect = dbContext.Projects
                .Include(p => p.Skills)
                .Include(p => p.Attachments)
                .Include(p => p.ProjectType)
                .ToList();

            if (!string.IsNullOrEmpty(titleOrganizationType))
            {
               
                var foundProj= allProgect.Where(p => p.Title.Contains(titleOrganizationType)
                                                    || p.Organization != null && p.Organization.Contains(titleOrganizationType)
                                                    || p.ProjectType.NameType.Contains(titleOrganizationType));
              
                if (foundProj.Any())
                {
                    allProgect = foundProj.ToList();
                }

            }

            var model = new ProjectsViewModel()
            {
                UserRole = userRole,

                ProjectsVm = allProgect.Select(pr => new ProjectViewModel()
                {

                    Id = pr.Id,
                    Title = pr.Title,
                    Description = pr.Description,
                    End = pr.End.HasValue ? pr.End.Value.ToString("dd.MM.yyyy hh:mm") : "",
                    
                    Start = pr.Start.HasValue ? pr.Start.Value.ToString("dd.MM.yyyy") : "",
                    Role = pr.Role,
                    Link = string.IsNullOrEmpty(pr.Link) ? "" : pr.Link,
                    Skills = pr.Skills.Count != 0 ? string.Join(", ", pr.Skills.Select(s => s.Name).ToArray()) : "",
                    Attachments = pr.Attachments.Count != 0 ? string.Join(", ", pr.Attachments.Select(s => s.File).ToArray()) : "",
                    ProjectType = pr.ProjectType.NameType,
                    Create = pr.Created.ToString("dd.MM.yyyy"),
                    Update = pr.Updated.ToString("dd.MM.yyyy")

                }).ToList()
            };



            return View("/Views/Projects/Index.cshtml", model);
        }

        public IActionResult AboutProject(Guid Id)
        {

            var prAll = dbContext.Projects
                .Include(p => p.Attachments)
                .Include(p => p.ProjectType)
                .Include(p => p.Skills)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == Id);

            var allTypes = this.dbContext.Types.ToList();

            var model = new AboutProjectVm()
            {

                Id = updatePr.Id,
                Title = updatePr.Title,
                Description = updatePr.Description,
                End = updatePr.End != null || updatePr.End.HasValue ? updatePr.End.Value.ToString("dd.MM.yyyy hh:mm") : "",

                Start = updatePr.Start != null || updatePr.Start.HasValue ? updatePr.Start.Value.ToString("dd.MM.yyyy") : "",
                Role = updatePr.Role,
                Link = string.IsNullOrEmpty(updatePr.Link) ? "" : updatePr.Link,
                // Skills = updatePr.Skills != null || updatePr.Skills.Count != 0 ? string.Join(", ", updatePr.Skills.Select(s => s.Name).ToArray()) : "",
                AttachmentVm = updatePr.Attachments.Select(f => new AboutFileVm()
                {
                    Id = f.Id,
                    File = f.File,
                    FileName = f.FileName,
                    Data = f.DateCreate.ToString("dd.MM.yyyy")
                }).ToList(),
                SkillsVm = updatePr.Skills == null ? new List<AboutSkillVm>() : updatePr.Skills.Select(s => new AboutSkillVm()
                {
                    Id = s.Id,
                    SkillName = s.Name,
                   
                }).ToList(),

                ProjectType = updatePr.ProjectType.NameType,
                Create = updatePr.Created.ToString("dd.MM.yyyy"),
                Update = updatePr.Updated.ToString("dd.MM.yyyy"),

              
            };

            return View("/Views/Projects/AboutProject.cshtml", model);

        }
        public IActionResult Create()
        {

            var allTypes = this.dbContext.Types.ToList();

            var model = new CreateProjectSubmitVm()
            {
                Types = allTypes.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.NameType

                }).ToList()
            };
            return View("/Views/Projects/Create.cshtml", model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateSubmit(CreateProjectSubmitVm model)
        {
            var roles = this.roleManager.Roles.ToList();

            var userName = HttpContext.User.Identity.Name;

            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            ////var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == userName);
            //var addedRoles = roles.Except(roles).ToList();

            string userRole = "";
            if (string.IsNullOrEmpty(userName))
            {
                var curentuser = allUsers.FirstOrDefault(u => u.AccountUser.Email == userName);

                if(curentuser != null)
                {
                    var roleUser = getRole(curentuser.AccountUser);
                }
                

                var type = this.dbContext.Types.FirstOrDefault(t => t.Id == model.TypeId);

               
                var newProduct = new Project()
                {
                    Id = Guid.NewGuid(),
                };



                if (ModelState.IsValid)
                {



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


                    return RedirectToAction(nameof(UploadFile), new { prId = newProduct.Id });
                }
                else
                {
                    ModelState.AddModelError("", "!!!");
                }


            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UploadFile(Guid prId)
        {
            var prAll = dbContext.Projects
                .Include(p => p.ProjectType)
                .Include(p => p.Attachments)
                .Include(p => p.Skills)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == prId);

            var model = new AddFileVm()
            {
                Id = updatePr.Id,
                Tytle = updatePr.Title
                
            };

            return View("/Views/Projects/UploadFile.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileSubmitAsync(AddFileVm model, IFormFile File)
        {

            var prAll = dbContext.Projects             
                .Include(p => p.Attachments)         
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == model.Id);

            if (File != null)
            {
                string name = File.FileName;
                string path = $"/files/{name}";
                string serverPath = $"{this.environment.WebRootPath}{path}";
                FileStream fs = new FileStream(serverPath, FileMode.Create,
                    FileAccess.Write);
                await File.CopyToAsync(fs);
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
            }

         

            return RedirectToAction(nameof(EditProject), new { Id = updatePr.Id });

        }

        public IActionResult AddSkill(Guid prId)
        {
            var prAll = dbContext.Projects
                .Include(p => p.ProjectType)
                .Include(p => p.Attachments)
                .Include(p => p.Skills)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == prId);

            var model = new AddFileVm()
            {
                Id = updatePr.Id,
                Tytle = updatePr.Title,


            };

            return View("/Views/Projects/AddSkill.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddSkillSubmit(AddFileVm model)
        {

            var prAll = dbContext.Projects
                .Include(p => p.Attachments)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == model.Id);
            var newskill = new Skill()
            {
                Id = Guid.NewGuid(),
                Name = model.Tytle,
               // ProjectId = updatePr.Id
            };


            if(updatePr.Skills == null)
            {
                updatePr.Skills = new List<Skill>()
                { };
            }
            dbContext.Skills.Add(newskill);
            updatePr.Skills.Add(newskill);

            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(EditProject), new { Id = updatePr.Id });

        }


            public IActionResult EditProject(Guid Id)
        {

            var prAll = dbContext.Projects
                .Include(p => p.Attachments)
                .Include(p => p.ProjectType)
                .Include(p => p.Skills)
                .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == Id);

            var allTypes = this.dbContext.Types.ToList();

            var model = new EditProjectVm()
            {

                Id = updatePr.Id,
                Title = updatePr.Title,
                Description = updatePr.Description,
                End = updatePr.End != null || updatePr.End.HasValue ? updatePr.End.Value.ToString("dd.MM.yyyy hh:mm") : "",
                
                Start = updatePr.Start != null || updatePr.Start.HasValue ? updatePr.Start.Value.ToString("dd.MM.yyyy") : "",
                Role = updatePr.Role,
                Link = string.IsNullOrEmpty(updatePr.Link) ? "" : updatePr.Link,
               // Skills = updatePr.Skills != null || updatePr.Skills.Count != 0 ? string.Join(", ", updatePr.Skills.Select(s => s.Name).ToArray()) : "",
                AttachmentVm = updatePr.Attachments.Select(f => new FileVm()
                {
                    Id = f.Id,
                    File = f.File,
                    FileName = f.FileName
                }).ToList(),
                SkillsVm = updatePr.Skills == null ? new List<SkillVm>()  : updatePr.Skills.Select(s => new SkillVm()
                {
                    Id = s.Id,
                    SkillName = s.Name
                }).ToList(),

                ProjectType = updatePr.ProjectType.NameType,
                Create = updatePr.Created.ToString("dd.MM.yyyy"),
                Update = updatePr.Updated.ToString("dd.MM.yyyy"),

                Types = allTypes.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.NameType

                }).ToList()
            };

            return View("/Views/Projects/EditProject.cshtml", model);

        }
        [HttpPost]
        public IActionResult EditProjectSubmit(EditProjectVm model)
        {

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
            if (endRes.IsValid)
            {
                DateTime dateEnd = new DateTime(startRes.Value.Year,
                  startRes.Value.Month,
                  startRes.Value.Day);



                dateEnd = dateEnd.AddHours(endRes.Value.Hour);
                dateEnd = dateEnd.AddMinutes(endRes.Value.Minute);

                updatePr.End = dateEnd;
            }
            var type = this.dbContext.Types.FirstOrDefault(t => t.Id == model.TypeId);

            updatePr.Title = model.Title;
            updatePr.Description = model.Description;
            updatePr.Organization = model.Organization;
            updatePr.Link = model.Link;

            
            updatePr.ProjectTypeId =type.Id;
            updatePr.Updated = DateTime.Now;

            dbContext.Update(updatePr);
            dbContext.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        private string getRole(AccountUser user)
        {
            var roles = this.roleManager.Roles.ToList();
         
            string userRole = "";
            //var roleAdmin = roles.FirstOrDefault(r => r.Name == "Admin");
            //var roleUser = roles.FirstOrDefault(r => r.Name == "User");

            var uRoles = this.userManager.GetRolesAsync(user).Result;

            if (uRoles.Contains("Admin"))
            {
                userRole = "Admin";
            }
            else if (uRoles.Contains("User"))
            {
                userRole = "User";
            }
            return userRole;
        }

       
        public IActionResult DeleteFile(Guid fileId, Guid prId)
        {
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == prId);

            var fileAll = dbContext.DbFiles.ToList();
            var fileFound = fileAll.FirstOrDefault(f => f.Id == fileId);

            updatePr.Attachments.Remove(fileFound);
            dbContext.DbFiles.Remove(fileFound);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(EditProject), new { Id = updatePr.Id });
        }
         
        public IActionResult DeleteSkill(Guid skillId, Guid prId)
        {
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();
            var updatePr = prAll.FirstOrDefault(p => p.Id == prId);

            var skillAll = dbContext.Skills.ToList();
            var skillFound = skillAll.FirstOrDefault(f => f.Id == skillId);

            updatePr.Skills.Remove(skillFound);
            dbContext.Skills.Remove(skillFound);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(EditProject), new { Id = updatePr.Id });
        }

        public IActionResult DeleteAll()
        {
            var prAll = dbContext.Projects
               .Include(p => p.Attachments)
               .ToList();

            foreach(var pr in prAll)
            {
                dbContext.Projects.Remove(pr);
            }
         
           
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
