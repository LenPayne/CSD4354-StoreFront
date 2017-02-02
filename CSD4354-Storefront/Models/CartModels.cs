using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD4354_Storefront.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public virtual List<ProductQty> Items { get; set; }
        public DateTime Date { get; set; }
        public double Discount { get; set; }
        public String Tracking { get; set; }
        public double TaxRate { get; set; }
        public int PaymentId { get; set; }
        public User Purchaser { get; set; }
    }

    public class ProductQty
    {
        public int Id { get; set; }
        public Product Item { get; set; }
        public int Quantity { get; set; }
    }
}