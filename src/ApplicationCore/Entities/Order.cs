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

        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string PhoneNumber { set; get; }

        public int ShipmentAddressID { set; get; }

        public string StreetAddress { set; get; }
        // public int ShipmentDetailID { set; get; }
        // public ShipmentDetail ShipmentDetail { set; get; }
        public TransactionStatus status { set; get; }

        public void AddShipmentDetail(ShipmentDetail detail)
        {
            LastName = detail.LastName;
            PhoneNumber = detail.PhoneNumber;
            StreetAddress = detail.StreetAddress;
            FirstName = detail.FirstName;
            ShipmentAddressID = detail.ID;
        }

        public bool isShippingDetailEmpty
        {
            get
            {
                if (LastName != null || LastName != null || StreetAddress != null || PhoneNumber != null)
                {
                    return false;
                }
                return true;
            }

        }

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