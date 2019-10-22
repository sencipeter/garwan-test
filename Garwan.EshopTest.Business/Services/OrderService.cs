using Garwan.EshopTest.Business.Services.Interfaces;
using Garwan.EshopTest.Common;
using Garwan.EshopTest.DataAccess;
using Garwan.EshopTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Garwan.EshopTest.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseObject<Order>> Get(int id)
        {
            var result = new ResponseObject<Order>();
            try
            {
                result.Result = (await _dbContext.Orders
                    .Include(x => x.Product)
                    .SingleOrDefaultAsync(r => r.Id == id));
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public async Task<ResponseObject<Order>> InsertOrder(Order order)
        {
            var result = new ResponseObject<Order>();
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                if (order.Id == 0)
                {
                    _dbContext.Orders.Add(order);
                }
                
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result.Result = null;
                result.Exception = ex;
            }
            if (result.Exception == null)
                result = await Get(order.Id);
            return result;
        }
    }
}
