using System;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Views.Shared.Components.CartProductListViewComponent
{
    public class CartProductListViewComponent : ViewComponent
    {
        ShopDBContext _context;
        private UserService _userServive;

        public CartProductListViewComponent(ShopDBContext context, UserService userService)
        {
            _context = context;
            _userServive = userService;
        }
        public IViewComponentResult Invoke()
        {
            var usr = this._userServive.GetByContext(HttpContext.User);
            // _context.Orders.Add/
            // List<Catergory> subCategory = _context.Catergories.Where((cat) => cat.ParrentId != null).ToList();
            // List<Catergory> model = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();
            // ViewBag.subCategory = subCategory;
            return View(); // Nếu khác Default.cshtml thì trả về View("abc", product) cho abc.cshtml
        }
    }
}
