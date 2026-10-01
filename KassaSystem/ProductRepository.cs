using System;
using System.Collections.Generic;
using System.IO;

namespace KassaSystem
{
    public class ProductRepository
    {
        public List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();

            string[] lines = File.ReadAllLines("products.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                Product product = new Product();

                product.Id = parts[0];
                product.Name = parts[1];
                product.Price = Convert.ToDouble(parts[2]);
                product.PriceType = parts[3];

                products.Add(product);
            }

            return products;
        }
    }
}