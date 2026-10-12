using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace KassaSystem.Tests
{
    public class AdminServiceTests
    {
        [Fact]
        public void ShowAdminMenu_AddProduct_ProductIsSaved()
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            TextReader originalInput = Console.In;
            TextWriter originalOutput = Console.Out;

            try
            {
                Directory.SetCurrentDirectory(tempDirectory);

                Console.SetIn(new StringReader(
                    "1\n463\nCoca-Cola\n30\nStyck\n"));

                Console.SetOut(new StringWriter());

                List<Product> products = new List<Product>();
                CampaignRepository campaignRepository = new CampaignRepository();
                AdminService adminService = new AdminService();

                adminService.ShowAdminMenu(products, campaignRepository);

                Assert.Single(products);
                Assert.Equal("463", products[0].Id);
                Assert.Equal("Coca-Cola", products[0].Name);
                Assert.Equal(30, products[0].Price);
                Assert.Equal("Styck", products[0].PriceType);

                Assert.True(File.Exists("products.txt"));

                string savedProduct = File.ReadAllText("products.txt");
                Assert.Equal("463;Coca-Cola;30;Styck", savedProduct.Trim());
            }
            finally
            {
                Console.SetIn(originalInput);
                Console.SetOut(originalOutput);
                Directory.SetCurrentDirectory(originalDirectory);

                Directory.Delete(tempDirectory, true);
            }
        }

        [Fact]
        public void ShowAdminMenu_UpdateProduct_ChangesAreSaved()
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            TextReader originalInput = Console.In;
            TextWriter originalOutput = Console.Out;

            try
            {
                Directory.SetCurrentDirectory(tempDirectory);

                Console.SetIn(new StringReader(
                    "2\n463\nCoca-Cola Zero\n25\n"));

                Console.SetOut(new StringWriter());

                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                List<Product> products = new List<Product> { product };

                CampaignRepository campaignRepository = new CampaignRepository();
                AdminService adminService = new AdminService();

                adminService.ShowAdminMenu(products, campaignRepository);

                Assert.Equal("Coca-Cola Zero", product.Name);
                Assert.Equal(25, product.Price);

                string savedProduct = File.ReadAllText("products.txt");
                Assert.Equal(
                    "463;Coca-Cola Zero;25;Styck",
                    savedProduct.Trim());
            }
            finally
            {
                Console.SetIn(originalInput);
                Console.SetOut(originalOutput);
                Directory.SetCurrentDirectory(originalDirectory);

                Directory.Delete(tempDirectory, true);
            }
        }

        [Fact]
        public void ShowAdminMenu_AddCampaign_CampaignIsSaved()
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            TextReader originalInput = Console.In;
            TextWriter originalOutput = Console.Out;

            try
            {
                Directory.SetCurrentDirectory(tempDirectory);

                string startDate = DateTime.Today.ToString("yyyy-MM-dd");
                string endDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");

                Console.SetIn(new StringReader(
                    $"3\n463\n15\n{startDate}\n{endDate}\n"));

                Console.SetOut(new StringWriter());

                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                List<Product> products = new List<Product> { product };

                string campaignFile = Path.Combine(tempDirectory, "campaigns.txt");
                CampaignRepository campaignRepository =
                    new CampaignRepository(campaignFile);

                AdminService adminService = new AdminService();

                adminService.ShowAdminMenu(products, campaignRepository);

                Assert.Single(campaignRepository.GetCampaigns());

                Campaign campaign = campaignRepository.GetCampaigns()[0];
                Assert.Equal(15, campaign.CampaignPrice);
                Assert.Equal("463", campaign.Product.Id);
                Assert.Equal(DateTime.Parse(startDate), campaign.StartDate);
                Assert.Equal(DateTime.Parse(endDate), campaign.EndDate);

                Assert.True(File.Exists(campaignFile));
            }
            finally
            {
                Console.SetIn(originalInput);
                Console.SetOut(originalOutput);
                Directory.SetCurrentDirectory(originalDirectory);

                Directory.Delete(tempDirectory, true);
            }
        }

        [Fact]
        public void ShowAdminMenu_RemoveCampaign_CampaignIsDeleted()
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDirectory);

            TextReader originalInput = Console.In;
            TextWriter originalOutput = Console.Out;

            try
            {
                Directory.SetCurrentDirectory(tempDirectory);

                string campaignFile = Path.Combine(tempDirectory, "campaigns.txt");

                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;
                product.PriceType = "Styck";

                Campaign campaign = new Campaign();
                campaign.Product = product;
                campaign.CampaignPrice = 15;
                campaign.StartDate = DateTime.Today;
                campaign.EndDate = DateTime.Today.AddDays(7);

                CampaignRepository campaignRepository =
                    new CampaignRepository(campaignFile);

                campaignRepository.AddCampaign(campaign);
                campaignRepository.SaveCampaigns();

                Console.SetIn(new StringReader("4\n1\n"));
                Console.SetOut(new StringWriter());

                AdminService adminService = new AdminService();

                adminService.ShowAdminMenu(
                    new List<Product> { product },
                    campaignRepository);

                Assert.Empty(campaignRepository.GetCampaigns());

                CampaignRepository newRepository =
                    new CampaignRepository(campaignFile);

                newRepository.LoadCampaigns(new List<Product> { product });

                Assert.Empty(newRepository.GetCampaigns());
            }
            finally
            {
                Console.SetIn(originalInput);
                Console.SetOut(originalOutput);
                Directory.SetCurrentDirectory(originalDirectory);

                Directory.Delete(tempDirectory, true);
            }
        }
    }
}