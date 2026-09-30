using System.Collections.Generic;

namespace KassaSystem
{
    public class Sale
    {
        public List<SaleProducts> SaleItems { get; set; }
        public Sale()
        {
            SaleItems = new List<SaleProducts>();
        }
    }

}

