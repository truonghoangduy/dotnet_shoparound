using System.Collections.Generic;

namespace ApplicationCore.Entities
{

    // TODO Order back-lock maybe
    class Order
    {
        public List<OrderDetail> OrderDetails { set; get; }
    }

    class OrderDetail
    {
        public string ProductID { set; get; }
        public int Quantity { set; get; }
    }
}