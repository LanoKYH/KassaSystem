
using System;
using System.IO;
using Xunit;

namespace KassaSystem.Tests
{
    public class ReceiptRepositoryTests
    {
        [Fact]
        public void SaveReceipt_WritesReceiptToFile()
        {
            string fileName = Path.Combine(
                Path.GetTempPath(),
                $"test_receipt_{Guid.NewGuid()}.txt");

            try
            {
                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                SaleProducts item = new SaleProducts();
                item.Product = product;
                item.Quantity = 2;
                item.UnitPrice = 20;

                Sale sale = new Sale();
                sale.SaleItems.Add(item);

                Receipt receipt = new Receipt();
                receipt.ReceiptNumber = 9999;
                receipt.Date = DateTime.Now;
                receipt.Sale = sale;
                receipt.Total = 40;

                ReceiptRepository repository =
                    new ReceiptRepository(fileName);

                repository.SaveReceipt(receipt);

                string savedText = File.ReadAllText(fileName);

                Assert.Contains("Kvittonummer: 9999", savedText);
                Assert.Contains("Coca-Cola", savedText);
                Assert.Contains("x 20 kr = 40 kr", savedText);
                Assert.Contains("TOTALT: 40 kr", savedText);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Fact]
        public void SaveReceipt_TwoReceipts_BothSavedInSameFile()
        {
            string fileName = Path.Combine(
                Path.GetTempPath(),
                $"test_receipt_{Guid.NewGuid()}.txt");

            try
            {
                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                SaleProducts item = new SaleProducts();
                item.Product = product;
                item.Quantity = 1;
                item.UnitPrice = 30;

                Sale sale = new Sale();
                sale.SaleItems.Add(item);

                Receipt firstReceipt = new Receipt();
                firstReceipt.ReceiptNumber = 1001;
                firstReceipt.Date = DateTime.Now;
                firstReceipt.Sale = sale;
                firstReceipt.Total = 30;

                Receipt secondReceipt = new Receipt();
                secondReceipt.ReceiptNumber = 1002;
                secondReceipt.Date = DateTime.Now;
                secondReceipt.Sale = sale;
                secondReceipt.Total = 30;

                ReceiptRepository repository =
                    new ReceiptRepository(fileName);

                repository.SaveReceipt(firstReceipt);
                repository.SaveReceipt(secondReceipt);

                string savedText = File.ReadAllText(fileName);

                Assert.Contains("Kvittonummer: 1001", savedText);
                Assert.Contains("Kvittonummer: 1002", savedText);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }


        [Fact]
        public void GetNextReceiptNumber_IncreasesAndPersistsNumber()
        {
            string fileName = Path.Combine(
                Path.GetTempPath(),
                $"test_receipt_number_{Guid.NewGuid()}.txt");

            try
            {
                ReceiptNumberRepository repository =
                    new ReceiptNumberRepository(fileName);

                int firstNumber = repository.GetNextReceiptNumber();
                int secondNumber = repository.GetNextReceiptNumber();

                ReceiptNumberRepository newRepository =
                    new ReceiptNumberRepository(fileName);

                int thirdNumber = newRepository.GetNextReceiptNumber();

                Assert.Equal(1001, firstNumber);
                Assert.Equal(1002, secondNumber);
                Assert.Equal(1003, thirdNumber);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}
