using ApplicationCore.Entities;
using ApplicationCore.Enum;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
    public class CartService : ICartService
    {
        private readonly ShopDBContext _context;

        public CartService(ShopDBContext context)
        {
            _context = context;
        }

        public ApplicationCore.Entities.Cart Info(AppUser user)
        {
            var query = _context.Carts.Include(order => order.CartDetails).ThenInclude(orderDetail => orderDetail.Product)
            .FirstOrDefault(order => order.AppUserID == user.Id);
            return query;
        }

        /// List<OrderDetail> orderDetails gather form HTTP Controller (cookie, session)
        /// ClaimsPrincipal userContext gather by HTTP Controller HTTP Context by Authorize URL 
        public async Task UpdateCart(AppUser user,
        CartDetail addedItem, CartAction action
        // List<OrderDetail> orderDetails
        )
        {
            var order = Info(user);
            if (order == null)
                order = await CreateCart(user);

            addedItem.Cart = order;
            addedItem.Cart.ID = order.ID;

            // empty user data
            var existItemIndex = isExist(addedItem.ProductID, order.CartDetails);

            // update / replace
            if (existItemIndex == -1)
            {
                _context.CartDetails.Add(addedItem);
            }
            else
            {
                var orderDetail = _context.CartDetails.FirstOrDefault(order => order.ID == existItemIndex);

                if (action == CartAction.Add_To_Cart)
                {
                    AddToCart(orderDetail);
                }
                else if (action == CartAction.Update)
                {
                    await ModifyOrderDetail(order, addedItem, orderDetail);
                }
            }
            _context.SaveChanges();


        }

        private void AddToCart(CartDetail orderDetail)
        {
            orderDetail.Quantity++;
            _context.CartDetails.Update(orderDetail);
        }

        private async Task ModifyOrderDetail(ApplicationCore.Entities.Cart order, CartDetail addedItem, CartDetail store)
        {
            if (addedItem.Quantity <= 0)
            {
                await Remove(order, addedItem.ProductID);
                return;
            }
            else if (store.Quantity == 1)
            {
                store.Quantity++;
            }
            else
            {
                store.Quantity = addedItem.Quantity;
                _context.CartDetails.Update(store);
            }
            _context.CartDetails.Update(store);
        }

        public async Task Remove(ApplicationCore.Entities.Cart order, int productId)
        {
            var orderRemoved = order.CartDetails.FirstOrDefault(orderDetail => orderDetail.Product.ID == productId);
            _context.CartDetails.Remove(orderRemoved);
            await _context.SaveChangesAsync();
        }


        public async Task Remove(AppUser userContext, int productId)
        {
            var cart = Info(userContext);
            var cartRemoved = cart.CartDetails.FirstOrDefault(orderDetail => orderDetail.Product.ID == productId);
            _context.CartDetails.Remove(cartRemoved);
            await _context.SaveChangesAsync();
            // var user = await _userService.GetByContext(userContext);
            // _context.OrderDetails.Remove(addedItem);

        }

        private int isExist(int productId, List<CartDetail> orderDetails)
        {
            if (orderDetails == null)
                return -1;
            var orderDetailRef = orderDetails.FirstOrDefault(orderDetail => orderDetail.ProductID == productId);
            if (orderDetailRef == null)
                return -1;
            // Return OrderDetail ID
            return orderDetailRef.ID;
        }

        private async Task<ApplicationCore.Entities.Cart> CreateCart(AppUser user)
        {
            var cart = new ApplicationCore.Entities.Cart()
            {
                AppUser = user,
                CartDetails = new List<CartDetail>(),
            };
            var result = await _context.Carts.AddAsync(cart);
            _context.SaveChanges();
            return cart;
        }
    }
}
