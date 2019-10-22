using System;
using System.Collections.Generic;
using System.Text;

namespace Garwan.EshopTest.Models
{
    public class Product:BaseModel
    {
        public int AnimalCategoryId { get; set; }
        public AnimalCategory AnimalCategory { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
