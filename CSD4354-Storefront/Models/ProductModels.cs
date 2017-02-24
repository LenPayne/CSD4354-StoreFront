using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSD4354_Storefront.Models
{
    public class Product    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int StockQuantity { get; set; }
        public String Location { get; set; }
        public String Colour { get; set; }
        public String Size { get; set; }
        public String ImageFilename { get; set; }
        public String ThumbnailFilename { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public Product Item { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductEditViewModel
    {
        public Product Item { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}