using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Garwan.EshopTest.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
