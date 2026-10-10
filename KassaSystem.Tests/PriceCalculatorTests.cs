using System;
using Xunit;

namespace KassaSystem.Tests
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void CalculatePrice_TwoProductsAt15_Returns30()
        {
            PriceCalculator calculator = new PriceCalculator();

            double result = calculator.CalculatePrice(15, 2);

            Assert.Equal(30, result);
        }

        [Fact]
        public void CalculatePrice_OneAndAHalfKgAt25_Returns37Point5()
        {
            PriceCalculator calculator = new PriceCalculator();

            double result = calculator.CalculatePrice(25, 1.5);

            Assert.Equal(37.5, result);


        }

        [Fact]
        public void CalculatePrice_NegativeQuantity_ThrowsException()
        {
            PriceCalculator calculator = new PriceCalculator();

            Assert.Throws<ArgumentException>(() =>
                calculator.CalculatePrice(15, -2));
        }

        [Fact]
        public void CalculatePrice_ZeroQuantity_ThrowsException()
        {
            PriceCalculator calculator = new PriceCalculator();

            Assert.Throws<ArgumentException>(() =>
                calculator.CalculatePrice(15, 0));
        }

        [Fact]
        public void CalculateTotal_UsesCampaignPrice()
        {
            Product product = new Product();
            product.Id = "463";
            product.Name = "Coca-Cola";
            product.Price = 30;

            SaleProducts item = new SaleProducts();
            item.Product = product;
            item.Quantity = 2;
            item.UnitPrice = 15;

            Sale sale = new Sale();
            sale.SaleItems.Add(item);

            SaleCalculator calculator = new SaleCalculator();

            double result = calculator.CalculateTotal(sale);

            Assert.Equal(30, result);
        }
    }
}