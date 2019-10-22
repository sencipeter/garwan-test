using System;
using System.Collections.Generic;
using System.Text;

namespace Garwan.EshopTest.Business.Requests
{
    public class ProductRequest:BasePagedRequest
    {        
        public ProductRequest()
        {
            Page = 1;
            PageSize = 10;
            
        }

    }
}
