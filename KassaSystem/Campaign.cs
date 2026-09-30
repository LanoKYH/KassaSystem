using System;


namespace KassaSystem
{
    public class Campaign
    {
        public Product Product { get; set; }
        public double CampaignDiscount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
