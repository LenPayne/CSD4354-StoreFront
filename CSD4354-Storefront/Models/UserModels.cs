using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD4354_Storefront.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Preferences { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String Province { get; set; }
        public String Postal { get; set; }
    }
}