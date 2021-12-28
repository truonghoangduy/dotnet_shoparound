using ApplicationCore.Entities;
using ApplicationCore.Enum;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ShopDBContext _context;

        public CartService(ShopDBContext context)
        {
            _context = context;
        }

        public Order Info(AppUser user)
        {
            var query = _context.Orders.Include(order => order.OrderDetails).ThenInclude(orderDetail => orderDetail.Product)
            .FirstOrDefault(order => order.AppUserID == user.Id);
            return query;
        }

        /// List<OrderDetail> orderDetails gather form HTTP Controller (cookie, session)
        /// ClaimsPrincipal userContext gather by HTTP Controller HTTP Context by Authorize URL 
        public async Task UpdateCart(AppUser user,
        OrderDetail addedItem, CartAction action
        // List<OrderDetail> orderDetails
        )
        {
            var order = Info(user);
            if (order == null)
                order = await CreateCart(user);

            addedItem.Order = order;
            addedItem.Order.ID = order.ID;

            // empty user data
            var existItemIndex = isExist(addedItem.ProductID, order.OrderDetails);

            // update / replace
            if (existItemIndex == -1)
            {
                _context.OrderDetails.Add(addedItem);
            }
            else
            {
                var orderDetail = _context.OrderDetails.FirstOrDefault(order => order.ID == existItemIndex);

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

        private void AddToCart(OrderDetail orderDetail)
        {
            orderDetail.Quantity++;
            _context.OrderDetails.Update(orderDetail);
        }

        private async Task ModifyOrderDetail(Order order, OrderDetail addedItem, OrderDetail store)
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
                _context.OrderDetails.Update(store);
            }
            _context.OrderDetails.Update(store);
        }

        public async Task Remove(Order order, int productId)
        {
            var orderRemoved = order.OrderDetails.FirstOrDefault(orderDetail => orderDetail.Product.ID == productId);
            _context.OrderDetails.Remove(orderRemoved);
            await _context.SaveChangesAsync();
        }


        public async Task Remove(AppUser userContext, int productId)
        {
            var order = Info(userContext);
            var orderRemoved = order.OrderDetails.FirstOrDefault(orderDetail => orderDetail.Product.ID == productId);
            _context.OrderDetails.Remove(orderRemoved);
            await _context.SaveChangesAsync();
            // var user = await _userService.GetByContext(userContext);
            // _context.OrderDetails.Remove(addedItem);

        }

        private int isExist(int productId, List<OrderDetail> orderDetails)
        {
            if (orderDetails == null)
                return -1;
            var orderDetailRef = orderDetails.FirstOrDefault(orderDetail => orderDetail.ProductID == productId);
            if (orderDetailRef == null)
                return -1;
            // Return OrderDetail ID
            return orderDetailRef.ID;
        }

        private async Task<Order> CreateCart(AppUser user)
        {
            var order = new Order()
            {
                AppUser = user,
                OrderDetails = new List<OrderDetail>(),
            };
            var result = await _context.Orders.AddAsync(order);
            _context.SaveChanges();
            return order;
        }

        public async Task<Order> Checkout()
        {
            return null;
        }

        // public void UpdateCart()
        // {

        // }

        // // Remove Product when Quantity <= 0
        // if (addedItem.Quantity <= 0)
        // {
        //     await Remove(order, addedItem.ProductID);
        // }
        // else
        // {
        //     // Update via Quantity
        //     orderDetail.Quantity++;
        //     _context.OrderDetails.Update(orderDetail);
        // }








    }
}
