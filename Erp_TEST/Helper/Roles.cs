using Erp_TEST.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Helper
{
    public  class Roles
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AccountUser> userManager;

        public Roles(RoleManager<IdentityRole> roleManager, UserManager<AccountUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public string GetRole(AccountUser user)
        {
            var roles = this.roleManager.Roles.ToList();

            string userRole = "";
           

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
    }
}
