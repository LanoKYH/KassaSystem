using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace KassaSystem.Tests
{
    public class CampaignRepositoryTests
    {
        [Fact]
        public void GetActiveCampaign_TwoActiveCampaigns_ReturnsCheapest()
        {
            Product product = new Product();
            product.Id = "463";
            product.Name = "Coca-Cola";
            product.Price = 30;

            Campaign expensiveCampaign = new Campaign();
            expensiveCampaign.Product = product;
            expensiveCampaign.CampaignPrice = 20;
            expensiveCampaign.StartDate = DateTime.Now.AddDays(-1);
            expensiveCampaign.EndDate = DateTime.Now.AddDays(1);

            Campaign cheapCampaign = new Campaign();
            cheapCampaign.Product = product;
            cheapCampaign.CampaignPrice = 15;
            cheapCampaign.StartDate = DateTime.Now.AddDays(-1);
            cheapCampaign.EndDate = DateTime.Now.AddDays(1);

            CampaignRepository repository = new CampaignRepository();
            repository.AddCampaign(expensiveCampaign);
            repository.AddCampaign(cheapCampaign);

            Campaign result = repository.GetActiveCampaign(product);

            Assert.NotNull(result);
            Assert.Equal(15, result.CampaignPrice);
        }

        [Fact]
        public void IsActive_CampaignOnEndDate_ReturnsTrue()
        {
            Campaign campaign = new Campaign();
            campaign.StartDate = DateTime.Today.AddDays(-2);
            campaign.EndDate = DateTime.Today;

            Assert.True(campaign.IsActive());
        }

        [Fact]
        public void SaveCampaigns_ThenLoadCampaigns_CampaignIsRestored()
        {
            string fileName = Path.GetTempFileName();

            try
            {
                Product product = new Product();
                product.Id = "463";
                product.Name = "Coca-Cola";
                product.Price = 30;

                Campaign campaign = new Campaign();
                campaign.Product = product;
                campaign.CampaignPrice = 15;
                campaign.StartDate = DateTime.Today;
                campaign.EndDate = DateTime.Today.AddDays(7);

                CampaignRepository repository = new CampaignRepository(fileName);
                repository.AddCampaign(campaign);
                repository.SaveCampaigns();

                CampaignRepository newRepository = new CampaignRepository(fileName);
                newRepository.LoadCampaigns(new List<Product> { product });

                Assert.Single(newRepository.GetCampaigns());
                Assert.Equal(15, newRepository.GetCampaigns()[0].CampaignPrice);
                Assert.Equal("463", newRepository.GetCampaigns()[0].Product.Id);
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}