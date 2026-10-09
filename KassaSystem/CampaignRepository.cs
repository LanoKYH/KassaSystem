using System;
using System.Collections.Generic;
using System.IO;

namespace KassaSystem
{
    public class CampaignRepository
    {
        private List<Campaign> campaigns = new List<Campaign>();
        private string fileName = "campaigns.txt";

        public void AddCampaign(Campaign campaign)
        {
            campaigns.Add(campaign);
        }

        public void RemoveCampaign(Campaign campaign)
        {
            campaigns.Remove(campaign);
        }

        public void SaveCampaigns()
        {
            List<string> lines = new List<string>();

            foreach (Campaign campaign in campaigns)
            {
                lines.Add($"{campaign.Product.Id};{campaign.CampaignPrice};{campaign.StartDate:yyyy-MM-dd};{campaign.EndDate:yyyy-MM-dd}");
            }

            try
            {
                File.WriteAllLines(fileName, lines);
                Console.WriteLine("Kampanjer sparade.");
            }
            catch
            {
                Console.WriteLine("Kunde inte spara kampanjerna.");
            }
        }


        public void LoadCampaigns(List<Product> products)
        {
            campaigns.Clear();

            if (!File.Exists(fileName))
            {
                return;
            }

            string[] lines;

            try
            {
                lines = File.ReadAllLines(fileName);
            }
            catch
            {
                Console.WriteLine("Kunde inte läsa campaigns.txt.");
                return;
            }

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                if (parts.Length != 4)
                {
                    continue;
                }

                Product product = products.Find(p => p.Id == parts[0]);

                if (product == null)
                {
                    continue;
                }

                if (!double.TryParse(parts[1], out double campaignPrice))
                {
                    continue;
                }

                if (!DateTime.TryParse(parts[2], out DateTime startDate))
                {
                    continue;
                }

                if (!DateTime.TryParse(parts[3], out DateTime endDate))
                {
                    continue;
                }

                Campaign campaign = new Campaign();
                campaign.Product = product;
                campaign.CampaignPrice = campaignPrice;
                campaign.StartDate = startDate;
                campaign.EndDate = endDate;

                campaigns.Add(campaign);
            }
        }

        public List<Campaign> GetCampaigns()
        {
            return campaigns;
        }

        public Campaign GetActiveCampaign(Product product)
        {
            Campaign bestCampaign = null;

            foreach (Campaign campaign in campaigns)
            {
                if (campaign.Product.Id == product.Id && campaign.IsActive())
                {
                    if (bestCampaign == null ||
                        campaign.CampaignPrice < bestCampaign.CampaignPrice)
                    {
                        bestCampaign = campaign;
                    }
                }
            }

            return bestCampaign;
        }
    }
}