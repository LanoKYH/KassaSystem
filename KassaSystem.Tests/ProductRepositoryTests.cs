using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace KassaSystem.Tests
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void LoadProducts_ValidFile_LoadsProductCorrectly()
        {
            string fileName = Path.GetTempFileName();

            try
            {
                File.WriteAllText(fileName, "463;Coca-Cola;30;Styck");

                ProductRepository repository = new ProductRepository(fileName);

                var products = repository.LoadProducts();

                Assert.Single(products);
                Assert.Equal("463", products[0].Id);
                Assert.Equal("Coca-Cola", products[0].Name);
                Assert.Equal(30, products[0].Price);
                Assert.Equal("Styck", products[0].PriceType);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void SaveProducts_ThenLoadProducts_ProductIsRestored()
        {
            string fileName = Path.GetTempFileName();

            try
            {
                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                ProductRepository repository = new ProductRepository(fileName);

                repository.SaveProducts(new System.Collections.Generic.List<Product> { product });

                var products = repository.LoadProducts();

                Assert.Single(products);
                Assert.Equal("463", products[0].Id);
                Assert.Equal("Coca-Cola", products[0].Name);
                Assert.Equal(30, products[0].Price);
                Assert.Equal("Styck", products[0].PriceType);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void LoadProducts_InvalidPrice_SkipsProduct()
        {
            string fileName = Path.GetTempFileName();

            try
            {
                File.WriteAllText(fileName, "463;Coca-Cola;abc;Styck");

                ProductRepository repository = new ProductRepository(fileName);

                var products = repository.LoadProducts();

                Assert.Empty(products);
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}
