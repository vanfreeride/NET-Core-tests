using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.infrastructure.DbObjects;

namespace webapi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userMgr,
                SignInManager<IdentityUser> signInMgr) {
            userManager = userMgr;
            signInManager = signInMgr;
            IdentitySeedData.EnsurePopulated(userMgr).Wait();
        }

        [HttpGet]
        [Route("login")]
        public ViewResult Login() {
            return View(new LoginViewModel ());
        }

        [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<JsonResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid) {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
                if (user != null) {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                            loginModel.Password, false, false)).Succeeded) {
                        return Json(true);
                    }
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
            return Json(false);
        }
        [Route("logout")]
        public async Task<RedirectResult> Logout(string returnUrl = "/") {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }

    public class LoginViewModel
    {
        [Required]
        public string Name  { get; set; }
        [Required]
        public string Password  { get; set; }
    }
}