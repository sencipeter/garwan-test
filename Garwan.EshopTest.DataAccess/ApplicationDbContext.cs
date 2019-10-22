using Garwan.EshopTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Garwan.EshopTest.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
         
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AnimalCategory> AnimalCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AnimalCategory>().HasData(
                new AnimalCategory { Id = 1, Name = "Dogs" },
                new AnimalCategory { Id = 2, Name = "Cats" },
                new AnimalCategory { Id = 3, Name = "Other" });
        }

        public void SeedTestData()
        {
            if (!Products.Any())
            {
                var rand = new Random((int)DateTime.Now.Ticks);
                for (var i = 1; i <= 100; i++)
                {
                    var categoryId = rand.Next(1, 4);
                    Products.Add(new Product
                    {
                        Name = "Product " + i,
                        AnimalCategoryId = categoryId,
                        Price = rand.Next(10, 1000),
                        Description = "Description of product " + i,
                    });
                }
                SaveChanges();
            }
        }
    }
}
