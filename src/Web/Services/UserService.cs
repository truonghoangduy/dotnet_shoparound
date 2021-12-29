using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Web.Models;
using System.Security.Claims;
using ApplicationCore.Interfaces;

namespace Web.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IEnumerable<AppUser> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AppUser> GetByContext(ClaimsPrincipal userContext)
        {
            return await this._userManager.GetUserAsync(userContext);
        }

        public async Task<AppUser> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }
            return user;

        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel usr)
        {
            var user = new AppUser { UserName = usr.Email, Email = usr.Email };
            var result = await _userManager.CreateAsync(user, usr.Password);
            return result;
            // throw new System.NotImplementedException();
        }
        public async Task<SignInResult> SignInWithPassword(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task<string> RegisterConfirmation(AppUser usr)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(usr);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            // var callbackURL = Url.Page()
            return code;
        }
    }
}