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
                    double total = 0;

                    while (true)
                    {
                        Console.WriteLine("Skanna produkt:");
                        string input = Console.ReadLine();

                        if (input == "PAY")
                        {
                            break;
                        }

                        string[] parts = input.Split(' ');

                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Fel format. Skriv: produktID antal");
                            continue;
                        }

                        string productId = parts[0];

                        if (!double.TryParse(parts[1], out double quantity))
                        {
                            Console.WriteLine("Antalet måste vara ett nummer.");
                            continue;
                        }

                        if (quantity <= 0)
                        {
                            Console.WriteLine("Antalet måste vara större än 0.");
                            continue;
                        }

                        Product product = products.Find(p => p.Id == productId);

                        if (product == null)
                        {
                            Console.WriteLine("Produkten finns inte.");
                            continue;
                        }

                        SaleProducts saleProduct = new SaleProducts();
                        saleProduct.Product = product;
                        saleProduct.Quantity = quantity;

                        sale.SaleItems.Add(saleProduct);
                       
                        double currentTotalPrice = product.Price * quantity;
                        total += currentTotalPrice;

                        Console.WriteLine($"{product.Name}");
                        Console.WriteLine($"{quantity} {product.PriceType} x {product.Price} kr = {currentTotalPrice} kr");
                        Console.WriteLine($"Total: {total} kr");
                    }

                    Console.WriteLine("Försäljning slutförd!");
                    Console.WriteLine($"Totalt: {total} kr");

                    Receipt receipt = new Receipt();

                    receipt.Date = DateTime.Now;
                    receipt.Sale = sale;
                    receipt.Total = total;

                    ReceiptRepository receiptRepository = new ReceiptRepository();
                    receiptRepository.SaveReceipt(receipt);

                    Console.WriteLine();
                    Console.WriteLine("===== KVITTO =====");
                    Console.WriteLine($"Datum: {receipt.Date}");

                    foreach (SaleProducts item in receipt.Sale.SaleItems)
                    {
                        double price = item.Product.Price * item.Quantity;

                        Console.WriteLine(
                            $"{item.Product.Name} - {item.Quantity} {item.Product.PriceType} x {item.Product.Price} kr = {price} kr");
                    }

                    Console.WriteLine($"TOTALT: {receipt.Total} kr");
                    Console.WriteLine("==================");

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

