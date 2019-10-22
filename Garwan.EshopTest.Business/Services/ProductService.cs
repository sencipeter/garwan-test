using Garwan.EshopTest.Business.Requests;
using Garwan.EshopTest.Business.Services.Interfaces;
using Garwan.EshopTest.Common;
using Garwan.EshopTest.DataAccess;
using Garwan.EshopTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Garwan.EshopTest.DataAccess.Extensions;

namespace Garwan.EshopTest.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Delete(List<int> ids)
        {
            var result = new Response();
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Products.RemoveRange(_dbContext.Products.Where(p => ids.Contains(p.Id)));
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                result.Exception = ex;
            }

            return result;
        }

        public async Task<ResponseObject<Product>> Get(int id)
        {
            var result = new ResponseObject<Product>();
            try
            {
                result.Result = (await _dbContext.Products
                    .Include(x => x.AnimalCategory)
                    .SingleOrDefaultAsync(r => r.Id == id));
            }
            catch (Exception ex)
            {
                result.Exception = ex;                
            }
            return result;
        }

        public async Task<ResponseList<Product>> Get(ProductRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderBy))
            {
                request.OrderBy = "Id";
            }

            if (string.IsNullOrEmpty(request.OrderDirection))
            {
                request.OrderDirection = "desc";
            }

            Expression<Func<Product, bool>> filter = null;
            if (!string.IsNullOrEmpty(request.Search))
            {
                request.Search = request.Search.ToLower();
                filter = f => f.Id.ToString().Contains(request.Search) || f.Name.ToLower().Contains(request.Search);
            }
            var result = new ResponseList<Product>();
            try
            {
                var rowCount = -1;
                IQueryable<Product> query = _dbContext.Products;
                if (filter != null)
                    query = query.Where(filter);
                if (request.OrderDirection == "asc")
                {
                    if (request.OrderBy != null)
                        query = query.OrderBy(request.OrderBy);
                }
                else
                {
                    if (request.OrderBy != null)
                        query = query.OrderByDescending(request.OrderBy);
                }

                if (request.Page != 0 && request.PageSize != 0)
                {
                    if (request.PageSize <= 0) request.PageSize = 20;
                    rowCount = await query.CountAsync();                    
                    if (rowCount <= request.PageSize || request.Page <= 0) request.Page = 1;
                    int excludedRows = (request.Page - 1) * request.PageSize;
                    query = query.Skip(excludedRows).Take(request.PageSize);
                }

                result.Result = await query.ToListAsync();
                result.TotalCount = rowCount;
                result.PageSize = request.PageSize;
                result.CurrentPage = request.Page;
            }
            catch (Exception ex)
            {
                result.Result = null;
                result.Exception = ex;
            }
            return result;
        }

        public async Task<ResponseObject<Product>> InsertUpdate(Product product)
        {
            var result = new ResponseObject<Product>();
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                if (product.Id == 0)
                {
                    _dbContext.Products.Add(product);
                }
                else
                {
                    if (_dbContext.Entry(product).State == EntityState.Detached)
                    {
                        _dbContext.Attach(product);
                    }
                    _dbContext.Entry(product).State = EntityState.Modified;
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
                result = await Get(product.Id);
            return result;
        }
    }
}
