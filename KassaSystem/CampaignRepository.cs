using System.Collections.Generic;

namespace KassaSystem
{
    public class CampaignRepository
    {
        private List<Campaign> campaigns = new List<Campaign>();

        public void AddCampaign(Campaign campaign)
        {
            campaigns.Add(campaign);
        }

        public void RemoveCampaign(Campaign campaign)
        {
            campaigns.Remove(campaign);
        }
        public List<Campaign> GetCampaigns()
        {
            return campaigns;
        }

        public Campaign GetActiveCampaign(Product product)
        {
            foreach (Campaign campaign in campaigns)
            {
                if (campaign.Product.Id == product.Id && campaign.IsActive())
                {
                    return campaign;
                }
            }

            return null;
        }
    }
}