using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Entities;
using System.Collections;

namespace ApplicationCore.Entities
{
    // Add profile data for application users by adding properties to the AppUser class
    public class AppUser : IdentityUser
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public int MainAddressId { set; get; }

        public List<Order> Orders { set; get; }
        public List<ShipmentDetail> ShipmentDetails { set; get; }
    }
}
