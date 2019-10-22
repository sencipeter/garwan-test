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
    public class ProductsTests
    {
        ApplicationDbContext _context;
        IProductService service;
        public ProductsTests()
        {           
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseSqlite("Filename=ProductsTests.db", o =>
                 {
                     o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                 })
               .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            service = new ProductService(_context);
        }

        [Fact]
        public async Task AddNewProduct()
        {            
            var originalCount = _context.Products.Count();
            var newProduct = new Models.Product
            {
                AnimalCategoryId = 1,
                Description = "Test insert description",
                Name = "Test insert name",
                Price = 10
            };
            var dbResult = await service.InsertUpdate(newProduct);
            var newCount  = _context.Products.Count();
            Assert.Equal(originalCount + 1, newCount);
            Assert.NotNull(dbResult.Result);
            Assert.Null(dbResult.Exception);
        }

        [Fact]
        public async Task UpdateProductsTest()
        {          
            var originalCount = _context.Products.Count();
            var newProduct = new Models.Product
            {
                AnimalCategoryId = 2,
                Description = "Test update description",
                Name = "Test update name",
                Price = 20
            };
            var insertedProduct = await service.InsertUpdate(newProduct);
            var updateProduct = (await service.Get(insertedProduct.Result.Id)).Result;

            var newCount = await _context.Products.CountAsync();
            
            Assert.NotNull(updateProduct);
            Assert.Equal(originalCount + 1, newCount);
            Assert.Equal("Test update name",updateProduct.Name);

            updateProduct.Name = "Updated product name";
            updateProduct.Description = "Updated product description";
            updateProduct.AnimalCategoryId = 3;
            updateProduct.Price = 30;

            var updateResult = await service.InsertUpdate(updateProduct);
            Assert.NotNull(updateResult.Result);
            Assert.Null(updateResult.Exception);

            var productInDb = (await service.Get(updateProduct.Id)).Result;
            Assert.NotNull(productInDb);
            Assert.Equal("Updated product name", productInDb.Name);
            Assert.Equal("Updated product description", productInDb.Description);
            Assert.Equal(3, productInDb.AnimalCategoryId);
            Assert.Equal(30M, productInDb.Price);
        }

        [Fact]
        public async Task DeleteProductTest()
        {
            var newProduct = new Models.Product
            {
                AnimalCategoryId = 1,
                Description = "Test delete description",
                Name = "Test delete name",
                Price = 10
            };
            var dbResult = await service.InsertUpdate(newProduct);
            Assert.NotNull(dbResult.Result);
            Assert.Null(dbResult.Exception);

            var dbResultDelete = await service.Delete(new List<int> { dbResult.Result.Id });
            var productFromDb = await service.Get(dbResult.Result.Id);                        
            Assert.Null(dbResultDelete.Exception);
            Assert.Null(productFromDb.Result);
        }
    }
}
