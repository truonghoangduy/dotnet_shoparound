using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ApplicationCore.Enum;

namespace ApplicationCore.Entities
{

    // TODO Order back-lock maybe
    public class Order : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public String AppUserID { set; get; }
        public AppUser AppUser { set; get; }
        public List<OrderDetail> OrderDetails { set; get; }

        public int ShipmentDetailId { set; get; }
        public ShipmentDetail ShipmentDetail { set; get; }
        public TransactionStatus status { set; get; }


        public bool isEmpty
        {
            get
            {
                if (this.OrderDetails.Count > 0)
                    return false;
                return true;
            }
        }
        public int Total
        {
            get
            {
                if (this.OrderDetails == null)
                    return 0;
                return this.OrderDetails.Sum(order => order.Product.PromotionPrice * order.Quantity);
            }
        }
    }

    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public Order Order { set; get; }

        public int OrderId { set; get; }
        public int ProductID { set; get; }
        public Product Product { set; get; }

        public int Quantity { set; get; }

        public int OrderDetailTotalPrice
        {
            get
            {
                return this.Product.PromotionPrice * this.Quantity;
            }
        }
    }
}