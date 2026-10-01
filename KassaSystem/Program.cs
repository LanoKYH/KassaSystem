using System;
using System.Collections.Generic;

namespace KassaSystem
{
    class Program
    {
        static void Main()
        {
            List<Product> products = new ProductRepository().LoadProducts();

            foreach (Product product in products)
            {
                Console.WriteLine($"{product.Id} - {product.Name} - {product.Price} kr - {product.PriceType}");
            }
        }
    }
}