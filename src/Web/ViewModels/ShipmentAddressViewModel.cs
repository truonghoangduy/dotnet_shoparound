using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
namespace Web.ViewModels
{
    public class ShipmentAddressViewModel
    {
        public String StreetAddress { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string PhoneNumber { set; get; }


        public ShipmentDetail getShipmentDetailFromAppUser(AppUser user)
        {
            var shipmentDetail = new ShipmentDetail().FormAppUser(user);
            shipmentDetail.StreetAddress = this.StreetAddress;
            shipmentDetail.FirstName = this.FirstName;
            shipmentDetail.PhoneNumber = this.PhoneNumber;
            shipmentDetail.LastName = this.LastName;
            return shipmentDetail;
        }
    }

    public class ShipmentAddressViewModelList
    {
        public List<ShipmentAddressViewModel> data { set; get; }
    }
}
