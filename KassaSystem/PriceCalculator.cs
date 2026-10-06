using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
