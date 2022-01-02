using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    public class ShipmentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public String AppUserID { set; get; }
        public AppUser AppUser { set; get; }
        public String StreetAddress { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string PhoneNumber { set; get; }

        public ShipmentDetail FormAppUser(AppUser user)
        {
            // this.AppUser = user;
            this.AppUserID = user.Id;
            this.FirstName = "";
            this.PhoneNumber = "";
            return this;
        }
    }
}