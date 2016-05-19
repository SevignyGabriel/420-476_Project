using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _420_476_Project.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<_420_476_Project.Models.Products> products { get; set; }
        public IEnumerable<_420_476_Project.Models.Orders> orders { get; set; }
        public IEnumerable<_420_476_Project.Models.OrderDetails> orderDetails { get; set; }
        public List<int> quantities = new List<int>();
    }
}