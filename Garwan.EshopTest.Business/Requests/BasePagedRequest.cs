using System;
using System.Collections.Generic;
using System.Text;

namespace Garwan.EshopTest.Business.Requests
{    
    public class BasePagedRequest    
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public string Search { get; set; }
    }
}
