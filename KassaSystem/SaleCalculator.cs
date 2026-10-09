namespace KassaSystem
{
    public class SaleCalculator
    {
        public double CalculateTotal(Sale sale)
        {
            double total = 0;

            foreach (SaleProducts item in sale.SaleItems)
            {
                total += item.UnitPrice * item.Quantity;
            }

            return total;
        }
    }
}