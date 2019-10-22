using Garwan.EshopTest.Common;
using Garwan.EshopTest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Garwan.EshopTest.Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseObject<Order>> InsertOrder(Order order);
        Task<ResponseObject<Order>> Get(int id);
    }
}
