using System;
using System.Collections.Generic;

namespace KassaSystem
{
    class Program
    {
        static void Main()
        {
            List<Product> products = new ProductRepository().LoadProducts();

            Console.WriteLine("=== KASSASYSTEM ===");
            Console.WriteLine("1. Ny försäljning");
            Console.WriteLine("2. Admin panel");
            Console.WriteLine("0. Avsluta");
            Console.WriteLine();
            Console.WriteLine("Välj ett alternativ:");
            string choice = Console.ReadLine();



            switch (choice)
            {
                case "1":
                    Sale sale = new Sale();
                    Console.WriteLine("Skanna produkt:");
                    string input = Console.ReadLine();

                    string[] parts = input.Split(' ');

                    string productId = parts[0];
                    double quantity = Convert.ToDouble(parts[1]);

                    Product product = products.Find(p => p.Id == productId);

                    SaleProducts saleProduct = new SaleProducts();
                    saleProduct.Product = product;
                    saleProduct.Quantity = quantity;

                    sale.SaleItems.Add(saleProduct); // skriv while loop senare

                    break;

                case "2":
                    Console.WriteLine("Välkommen till Admin Panel");
                    break;

                case "0":
                    Console.WriteLine("Välkommen åter!");
                    break;

                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
        }
    }
}

