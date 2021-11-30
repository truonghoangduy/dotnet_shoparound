using System.Collections.Generic;

namespace ApplicationCore.Entities
{

    // TODO Order back-lock maybe
    public class Order
    {
        public List<OrderDetail> OrderDetails { set; get; }
    }

    public class OrderDetail
    {
        public string ProductID { set; get; }
        public Product Product { set; get; }

        public int Quantity { set; get; }
    }
}