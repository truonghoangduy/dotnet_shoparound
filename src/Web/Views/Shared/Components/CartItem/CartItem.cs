using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Views.Shared.Components.CartItemViewComponent
{
    public class CartItemViewComponent : ViewComponent
    {
        CartService _cartService;
        UserService _userServive;

        public CartItemViewComponent(ICartService cartService, IUserService userService)
        {
            _cartService = (CartService)cartService;
            _userServive = (UserService)userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // TODO Fix get user info only once
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await this._userServive.GetByContext(HttpContext.User);
                var orderInfo = _cartService.Info(currentUser);
                ViewBag.CartInfo = orderInfo;
            }
            return View();
        }
    }


}
