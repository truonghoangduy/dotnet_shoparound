using Microsoft.AspNetCore.Mvc;
using Web.Services.User;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private UserService _userServive;
        public UserController(IUserService userService)
        {
            _userServive = (UserService)userService;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            await this._userServive.SignOut();
            return Redirect(returnUrl);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await this._userServive.SignInWithPassword(model);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View();
        }

        public async Task<IActionResult> RegisterConfirmation(string email, string returnUrl = null)
        {
            try
            {
                var user = await this._userServive.GetByEmail(email);
                var urlCode = await this._userServive.RegisterConfirmation(user);
                var callbackURL = Url.Page(
                    "/Account/ConfirmEmail", pageHandler: null, values: new
                    {
                        area = "Identity",
                        userId = user.Id,
                        code = urlCode,
                        returnUrl = returnUrl,
                        protocol = Request.Scheme
                    }
                );
                ViewBag.Email = user.Email;
                ViewBag.DisplaceConfirmAccountLink = true;
                ViewBag.EmailConfirmationLink = callbackURL;
                return View();
            }
            catch (System.Exception e)
            {
                throw new System.NotImplementedException();
            }

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await this._userServive.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("RegisterConfirmation", new
                    {
                        email = model.Email,
                        returnUrl = returnUrl
                    });
                    // await this._userServive.SignIn(model);
                    // return LocalRedirect(returnUrl);
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
            }
            return View();
        }
    }
}