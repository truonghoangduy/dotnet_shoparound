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
    public class OrderController : Controller
    {
        private UserService _userServive;

        private CartService _cartService;
        private OrderService _orderService;
        private ShopDBContext _db_context;

        public OrderController(IUserService userService, ICartService cartService, ShopDBContext db_context, IOrderService orderService)
        {
            // lazy
            _userServive = (UserService)userService;
            _cartService = (CartService)cartService;
            _db_context = db_context;
            _orderService = (OrderService)orderService;
        }



        [Authorize]
        [Route("checkout")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var cartInfo = _cartService.Info(currentUser);
            var temp_order = _orderService.CreatePreviewOrder(cartInfo, currentUser);
            var order = _orderService.Info(currentUser, temp_order.ID);
            return RedirectToAction("Index", new
            {
                id = order.ID
            });
        }

        [Authorize]
        [Route("checkout/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var order = _orderService.Info(currentUser, id);
            return View("Index", order);
        }


        [Authorize]
        [HttpGet]
        [Route("checkout/status/{status}")]
        public async Task<IActionResult> UpdateStatus(int status)
        {
            var currentUser = await this._userServive.GetByContext(HttpContext.User);
            var order = _orderService.Info(currentUser);
            order = _orderService.UpdateOrderStatus((TransactionStatus)status, order);
            return RedirectToAction("Index", new
            {
                id = order.ID
            });
        }




    }
}