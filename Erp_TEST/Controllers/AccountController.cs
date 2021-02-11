using Erp_TEST.Data;
using Erp_TEST.Models;
using Erp_TEST.Models.DbModel;
using Erp_TEST.Models.ViewModel;
using Erp_TEST.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AccountUser> userManager;
        private readonly SignInManager<AccountUser> signInManager;
        private readonly ApplicationDbContext dbContext;

        public AccountController(UserManager<AccountUser> userManager,
                                 SignInManager<AccountUser> signInManager,
                                 ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                AccountUser accountUser = new AccountUser { Email = model.Email, UserName = model.Email };
                // add
                var res = await this.userManager.CreateAsync(accountUser, model.Password);
                if (res.Succeeded)
                {
                    // установка куки
                    // await this.signInManager.SignInAsync(accountUser, false);
                    await this.signInManager.SignInAsync(accountUser, false);
                 
                    await this.userManager.AddToRoleAsync(accountUser, "User");

                    string emailConfirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(accountUser);

                    var confirmResult = await this.userManager.ConfirmEmailAsync(accountUser, emailConfirmationToken);

                    if(confirmResult.Succeeded)
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
                   

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                var user = await this.userManager.FindByIdAsync("506330e7-25f7-4c18-85ff-e1bce8f89c1a");

                //var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

                ////  var res = await this.userManager.ResetPasswordAsync(user, token, "MyN3wP@ssw0rd");
                //await this.userManager.RemovePasswordAsync(user);
               // await this.userManager.AddPasswordAsync(user, model.Password);

                // return RedirectToAction("Index", "Home");
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}

