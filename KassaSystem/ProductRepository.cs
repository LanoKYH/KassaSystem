using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace KassaSystem
{
    public class ProductRepository
    {
        private readonly string fileName;

        public ProductRepository(string fileName = "products.txt")
        {
            this.fileName = fileName;
        }
        public List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();

            string[] lines;

            try
            {
                lines = File.ReadAllLines(fileName);
            }
            catch
            {
                Console.WriteLine("Kunde inte läsa products.txt.");
                return products;
            }

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                if (parts.Length != 4)
                {
                    Console.WriteLine("Fel format i products.txt.");
                    continue;
                }

                Product product = new Product();

                product.Id = parts[0];
                product.Name = parts[1];

                if (!double.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out double price))
                {
                    Console.WriteLine($"Fel pris för produkten {parts[0]}.");
                    continue;
                }

                product.Price = price;
                product.PriceType = parts[3];

                products.Add(product);
            }

            return products;
        }


        public void SaveProducts(List<Product> products)
        {
            List<string> lines = new List<string>();

            foreach (Product product in products)
            {
                lines.Add($"{product.Id};{product.Name};{product.Price.ToString(CultureInfo.InvariantCulture)};{product.PriceType}");
            }

            try
            {
                File.WriteAllLines(fileName, lines);
                Console.WriteLine("Produkter sparade.");
            }
            catch (IOException)
            {
                Console.WriteLine("Kunde inte spara produkterna.");
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Åtkomst nekad när produkterna skulle sparas.");
                throw;
            }
        }

    }
}