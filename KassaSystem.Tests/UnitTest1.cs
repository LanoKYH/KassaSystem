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
    }
}