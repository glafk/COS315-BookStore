using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //Check if there are any errors in the registration form
            if(ModelState.IsValid)
            {
                //Create new user in the database
                var user = new User
                {
                    UserName = model.Email,
                    Address = model.Address,
                    FullName = model.FullName,
                    CreditCardLegalName = model.CreditCardLegalName,
                    CreditCardSVC = model.CeditCardCVS
                };
                var newUserCreation = await userManager.CreateAsync(user, model.Password);

                //Sign in the user if creation was successful
                if(newUserCreation.Succeeded)
                {
                    await signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Add error to be displayed to the userif the there are errors in the form
                    ModelState.AddModelError("PasswordReq", "Password must be at least 8 characters long and include an uppercase character, a lowercase character, a number and a special symbol");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                //Attemp to sign in the user with the given password
                var signInResult = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if(signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LogOnError", "Username or password is incorrect.");
                }
            }
            return View(model);
        }
    }
}