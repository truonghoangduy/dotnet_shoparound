using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entities;
using ApplicationCore.Enum;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopDBContext _context;

        public OrderService(ShopDBContext context)
        {
            _context = context;
        }

        public List<Order> GetOrders(AppUser user)
        {
            var order = _context.Orders.Include(order => order.OrderDetails).ThenInclude(orderDetail => orderDetail.Product).Include(order => order.AppUser)
            .OrderByDescending(x => x.CreatedAt)
            .Where(order => order.AppUserID == user.Id
            // && order.status == TransactionStatus.WaitForComformation 
            ).ToList();
            return order;
        }

        public Order CreatePreviewOrder(Cart cart, AppUser user)
        {
            Order order = null;

            // Remvoe previous order
            var previousOrderInfo = Info(user, TransactionStatus.WaitForComformation);
            if (previousOrderInfo != null)
            {
                _context.OrderDetails.RemoveRange(previousOrderInfo.OrderDetails);
                previousOrderInfo.OrderDetails = cart.CartDetails.Select(detail => new OrderDetail()
                {
                    OrderId = previousOrderInfo.ID,
                    Product = detail.Product,
                    Quantity = detail.Quantity
                }).ToList();
                _context.Update(previousOrderInfo);
                _context.SaveChanges();
                return previousOrderInfo;
            }

            // Create new Order corresponding to Cart
            order = new Order()
            {
                AppUser = cart.AppUser,
                AppUserID = cart.AppUserID,
            };
            order.status = TransactionStatus.WaitForComformation;
            order.OrderDetails = new List<OrderDetail>();
            _context.Orders.Update(order);
            _context.SaveChanges();
            order.OrderDetails = cart.CartDetails.Select(detail => new OrderDetail()
            {
                OrderId = order.ID,
                Product = detail.Product,
                Quantity = detail.Quantity
            }).ToList();
            _context.SaveChanges();
            return order;
        }


        public Order Info(AppUser user)
        {
            var order = _context.Orders.Include(order => order.OrderDetails).ThenInclude(orderDetail => orderDetail.Product).Include(order => order.AppUser)
            .FirstOrDefault(order => order.AppUserID == user.Id
            // && order.status == TransactionStatus.WaitForComformation 
            );
            return order;
        }
        public Order Info(AppUser user, TransactionStatus status)
        {
            var order = _context.Orders.Include(order => order.OrderDetails).ThenInclude(orderDetail => orderDetail.Product).Include(order => order.AppUser)
            .FirstOrDefault(order => order.AppUserID == user.Id
            && order.status == status
            );
            return order;
        }
        public Order Info(AppUser user, int orderID)
        {
            var order = _context.Orders.Include(order => order.OrderDetails).ThenInclude(orderDetail => orderDetail.Product).Include(order => order.AppUser)
            .FirstOrDefault(order => order.AppUserID == user.Id && order.ID == orderID
            // && order.status == TransactionStatus.WaitForComformation 
            );
            return order;
        }

        public Order UpdateOrderStatus(TransactionStatus status, Order order)
        {
            order.status = status;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return order;
        }

        public void UpdateOrderAddress(AppUser currentUser, Order usr_order, ShipmentDetail shipmentDetail)
        {
            var update_address_cursor = _context.Orders.FirstOrDefault(order => order.ID == usr_order.ID && order.AppUserID == currentUser.Id);
            update_address_cursor.ShipmentDetailId = shipmentDetail.ID;
            _context.Orders.Update(update_address_cursor);
            _context.SaveChanges();
        }

        public void RemoveOrder(AppUser currentUser, Order usr_order)
        {
            var remove_cursor = _context.Orders.FirstOrDefault(order => order.ID == usr_order.ID && order.AppUserID == currentUser.Id);
            _context.Orders.Remove(remove_cursor);
            _context.SaveChanges();
        }

        internal void UpdateOrderStatus(int status, AppUser currentUser)
        {
            throw new NotImplementedException();
        }
    }
}