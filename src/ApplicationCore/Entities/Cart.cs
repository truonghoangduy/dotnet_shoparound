using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApplicationCore.Entities
{

    // TODO Order back-lock maybe
    public class Cart : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public String AppUserID { set; get; }
        public AppUser AppUser { set; get; }
        public List<CartDetail> CartDetails { set; get; }

        public bool isEmpty
        {
            get
            {
                if (this.CartDetails.Count > 0)
                    return false;
                return true;
            }
        }
        public int Total
        {
            get
            {
                if (this.CartDetails == null)
                    return 0;
                return this.CartDetails.Sum(order => order.Product.PromotionPrice * order.Quantity);
            }
        }


    }

    public class CartDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public Cart Cart { set; get; }
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