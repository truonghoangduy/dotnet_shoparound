using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Web.Helper;
using ApplicationCore.Entities;
using ApplicationCore.Enum;
using Web.Services;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private UserService _userServive;

        private CartService _cartService;
        private ShopDBContext _db_context;
        public CartController(IUserService userService, ICartService cartService, ShopDBContext db_context)
        {
            // lazy
            _userServive = (UserService)userService;
            _cartService = (CartService)cartService;
            _db_context = db_context;


        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var orderInfo = _cartService.Info(currentUser);
            return View(orderInfo);
        }


        // REST API 
        [Authorize]
        [HttpGet]
        [Route("cart/info")]
        public async Task<Cart> InfoAsync()
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var orderInfo = _cartService.Info(currentUser);
            return orderInfo;
        }

        [Authorize]
        [Route("cart/add/{id}")]
        public async Task<IActionResult> Add(int Id, int Amount = 1)
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var product = _db_context.Products.Find(Id);
            // var session_cart = SessionHelper.GetObjectFromJson<Order>(_session, "order_cart");
            var addedItem = new CartDetail() { Product = product, ProductID = product.ID, Quantity = Amount };
            await _cartService.UpdateCart(currentUser, addedItem, CartAction.Add_To_Cart);

            return RedirectToAction("index", "product", new { id = Id });
            // var Product = _context.Products.Find(id);
            // return View(Product);
        }


        [Authorize]
        [HttpGet]
        [Route("cart/update/{id}")]
        public async Task<IActionResult> UpdateCart(int Id, int Amount = 1)
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var product = _db_context.Products.Find(Id);
            // var session_cart = SessionHelper.GetObjectFromJson<Order>(_session, "order_cart");
            var addedItem = new CartDetail() { Product = product, ProductID = product.ID, Quantity = Amount };
            await _cartService.UpdateCart(currentUser, addedItem, CartAction.Update);

            return RedirectToAction("Index");
        }



        [Authorize]
        [HttpDelete]
        [Route("cart/remove/{productid}")]
        public async Task<IActionResult> Remove(int productid)
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);

            await this._cartService.Remove(currentUser, productid);
            return RedirectToAction("Index");
            // var Product = _context.Products.Find(id);
            // return View(Product);
        }

    }
}