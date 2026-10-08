namespace KassaSystem
{
    public class PriceCalculator
    {
        public double CalculatePrice(double price, double quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            return price * quantity;
        }

        public double CalculatePrice(Product product, double quantity, Campaign campaign)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            double price = product.Price;

            if (campaign != null && campaign.IsActive())
            {
                price = campaign.CampaignPrice;
            }

            return price * quantity;
        }
    }
}
