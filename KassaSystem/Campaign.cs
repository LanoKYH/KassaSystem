using System;


namespace KassaSystem
{
    public class Campaign
    {
        public Product Product { get; set; }
        public double CampaignPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive()
        {
            DateTime now = DateTime.Now;

            return now >= StartDate && now <= EndDate;
        }
    }
}
