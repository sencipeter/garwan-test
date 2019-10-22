using Garwan.EshopTest.Business.Requests;
using Garwan.EshopTest.Common;
using Garwan.EshopTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Garwan.EshopTest.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<ResponseObject<Product>> Get(int id);
        Task<ResponseObject<Product>> InsertUpdate(Product product);
        Task<Response> Delete(List<int> ids);
        Task<ResponseList<Product>> Get(ProductRequest request);
    }
}
