using System;
using System.Collections.Generic;
using System.Text;

namespace Garwan.EshopTest.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
