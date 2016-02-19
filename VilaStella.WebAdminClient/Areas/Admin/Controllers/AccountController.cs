using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Jint.Parser.Ast;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.WebAdminClient.Models;

namespace VilaStella.WebAdminClient.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController(IGenericRepositoy<ForgottenPassword> forgottenPasswords)
        {
            this.ForgottenPasswords = forgottenPasswords;
        }

        public AccountController(ApplicationUserManager userManager, IGenericRepositoy<ForgottenPassword> forgottenPasswords)
        {
            this.UserManager = userManager;
            this.ForgottenPasswords = forgottenPasswords;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this._userManager = value;
            }
        }

        public IGenericRepositoy<ForgottenPassword> ForgottenPasswords { get; private set; }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await this.UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await this.SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToLocal(returnUrl);
        }

        //[AllowAnonymous]
        //public ActionResult Register(string returnUrl)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel registerModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser user = new ApplicationUser()
        //        {
        //            UserName = registerModel.Email,
        //            Email = registerModel.Email
        //        };

        //        IdentityResult result = await UserManager.CreateAsync(user, registerModel.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //    }

        //    return View(registerModel);
        //}

        [AllowAnonymous]
        public async Task<ActionResult> ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgottenPassword(ForgotPasswordViewModel forgottenPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(forgottenPassword);
            }

            ApplicationUser user = await this.UserManager.FindByEmailAsync(forgottenPassword.Email);

            if (user != null)
            {
                string resetCode = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string passwordResetUrl = Url.Action("ResetPassword", "Account", new {area = "Admin", code = resetCode, email = user.Email},
                    protocol: Request.Url.Scheme);

                await
                    this.UserManager.SendEmailAsync(user.Id, "Reset Password",
                        $"Please reset your password by clicking <a href=\"{passwordResetUrl}\">here</a>");

                return View("PasswordResetEmailSent", model: user.Email);
            }

            return View("PasswordResetEmailNotFound", model: forgottenPassword.Email);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string email)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
            {
                return View("PasswordResetMismatch");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            ApplicationUser user = await this.UserManager.FindByEmailAsync(resetPassword.Email);

            if (user == null)
            {
                return View("PasswordResetEmailNotFound", model: resetPassword.Email);
            }

            IdentityResult result = await this.UserManager.ResetPasswordAsync(user.Id, resetPassword.Code, resetPassword.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account", new {area = "Admin"});
            }

            this.AddErrors(result);
            return View(resetPassword);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(this.UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private bool HasPassword()
        {
            var user = this.UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
        }
    }
}