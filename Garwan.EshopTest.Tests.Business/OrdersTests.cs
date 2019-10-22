using Garwan.EshopTest.Business.Services;
using Garwan.EshopTest.Business.Services.Interfaces;
using Garwan.EshopTest.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Garwan.EshopTest.Tests.Business
{
    public class OrdersTests
    {
        ApplicationDbContext _context;
        IOrderService service;
        IProductService productService;
        public OrdersTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseSqlite("Filename=OrdersTests.db", o =>
             {
                 o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
             })
              .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            service = new OrderService(_context);
            productService = new ProductService(_context);
        }

        [Fact]
        public async Task AddNewOrderTest()
        {
            
            var newProduct = new Models.Product
            {
                AnimalCategoryId = 1,
                Description = "Test order description",
                Name = "Test order name",
                Price = 10
            };
            var dbResultProduct = await productService.InsertUpdate(newProduct);
            Assert.Null(dbResultProduct.Exception);

            var originalCount = await _context.Orders.CountAsync();
            var newOrder = new Models.Order
            {
               ProductId = dbResultProduct.Result.Id,
               ProductCount = 2,
               Time = DateTime.Now,
               TotalPrice = 2 * dbResultProduct.Result.Price,                
            };
            var dbResult = await service.InsertOrder(newOrder);
            var newCount = await _context.Orders.CountAsync();
            Assert.Equal(originalCount + 1, newCount);
            Assert.Equal(20, newOrder.TotalPrice);
            Assert.NotNull(dbResult.Result);
            Assert.Null(dbResult.Exception);
        }
    }
}
