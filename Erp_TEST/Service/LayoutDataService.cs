using Erp_TEST.Data;
using Erp_TEST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Service.LayoutData
{
    public interface ILayoutDataService
    {
        string GetUserRole();
        bool IsAuthenticated();
        bool IsIsSignedInUser();
        Task<Guid> UserIdAsync();
    }
    public class LayoutDataService : ILayoutDataService
    {
        private IHttpContextAccessor httpContextAccessor;
        private ApplicationDbContext dbContext;
        private UserManager<AccountUser> userManager;
        private SignInManager<AccountUser> signInManager;

        public LayoutDataService(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<AccountUser> userManager,
            SignInManager<AccountUser> signInManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public bool IsIsSignedInUser()
        {
            //var userName = this.httpContextAccessor.HttpContext.User.Identity.Name;
            //var res = this.signInManager.IsSignedIn(userName);

            throw new NotImplementedException();
        }

        public string GetUserRole()
        {
            var userName = this.httpContextAccessor.HttpContext.User.Identity.Name;
            string userRole = "";
            if (this.httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                userRole = "Admin";
            }
            else if (this.httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                userRole = "User";
            }
            return userRole;
        }

        public bool IsAuthenticated()
        {
            var userName = this.httpContextAccessor.HttpContext.User.Identity.Name;
            var isAuthenticated = this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            return isAuthenticated;
        }

        

        public Task<Guid> UserIdAsync()
        {
            throw new NotImplementedException();
        }

    
    }
}
