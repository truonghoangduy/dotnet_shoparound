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
using Infrastructure.Data;
using System.Linq;

namespace Web.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ShopDBContext _context;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ShopDBContext context
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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

        public async Task AddAddress(ShipmentDetail address)
        {
            _context.ShipmentDetails.Update(address);
            _context.SaveChanges();
        }

        public async Task<ShipmentDetail> GetAddressById(AppUser user, int ID)
        {
            var address = _context.ShipmentDetails.FirstOrDefault(address => address.AppUserID == user.Id && address.ID == ID);
            return address;
        }


        public async Task RemoveAddress(ShipmentDetail address)
        {
            _context.ShipmentDetails.Remove(address);
            await _context.SaveChangesAsync();
        }

        public List<ShipmentDetail> GetUserShipmentAddress(AppUser user)
        {
            var listOfAddress = _context.ShipmentDetails.Where(address => address.AppUserID == user.Id).ToList();
            return listOfAddress;
        }
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel usr)
        {
            var user = new AppUser { UserName = usr.Email, Email = usr.Email };
            var result = await _userManager.CreateAsync(user, usr.Password);
            return result;
            // throw new System.NotImplementedException();
        }

        public void UpdateAddress(List<ShipmentDetail> list_shipmentDetail, AppUser user)
        {
            // DUMP
            var oldShipmentDetails = _context.ShipmentDetails.Where(detail => detail.AppUserID == user.Id).ToList();
            _context.ShipmentDetails.RemoveRange(oldShipmentDetails);
            _context.ShipmentDetails.UpdateRange(list_shipmentDetail);
            _context.SaveChanges();
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