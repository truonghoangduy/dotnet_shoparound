using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ConfromOrderViewModel
    {
        [Required]
        [Display(Name = "Shipment Address")]
        public string ShipmentId { set; get; }

    }
}
