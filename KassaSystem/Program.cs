using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace KassaSystem
{
    class Program
    {
        static void Main()
        {
            List<Product> products = new ProductRepository().LoadProducts();
            CampaignRepository campaignRepository = new CampaignRepository();
            campaignRepository.LoadCampaigns(products);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== KASSASYSTEM ===");
                Console.WriteLine("1. Ny försäljning");
                Console.WriteLine("2. Admin panel");
                Console.WriteLine("0. Avsluta");

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

                            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                            if (parts.Length != 2)
                            {
                                Console.WriteLine("Fel format. Skriv: produktID antal");
                                continue;
                            }

                            string productId = parts[0];

                            if (!double.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out double quantity)) 
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

                            if (!product.IsAvailable)
                            {
                                Console.WriteLine("Produkten är inte tillgänglig för försäljning.");
                                continue;
                            }

                            Campaign activeCampaign = campaignRepository.GetActiveCampaign(product);

                            SaleProducts saleProduct = new SaleProducts();
                            saleProduct.Product = product;
                            saleProduct.Quantity = quantity;

                            double priceToUse = product.Price;

                            if (activeCampaign != null)
                            {
                                priceToUse = activeCampaign.CampaignPrice;
                            }

                            saleProduct.UnitPrice = priceToUse;

                            sale.SaleItems.Add(saleProduct);

                            PriceCalculator priceCalculator = new PriceCalculator();

                            double currentTotalPrice = priceCalculator.CalculatePrice(product, quantity, activeCampaign);

                            SaleCalculator saleCalculator = new SaleCalculator();
                            total = saleCalculator.CalculateTotal(sale);

                            Console.WriteLine($"{product.Name} - {quantity} {product.PriceType} x " + $"{saleProduct.UnitPrice:F2} kr = {currentTotalPrice:F2} kr");
                            Console.WriteLine($"Delsumma: {total:F2} kr");
                            Console.WriteLine();
                        }


                        if (sale.SaleItems.Count == 0)
                        {
                            Console.WriteLine("Försäljningen innehåller inga produkter.");
                            break;
                        }

                        Receipt receipt = new Receipt();

                        try
                        {
                            ReceiptNumberRepository numberRepository =
                                new ReceiptNumberRepository();

                            receipt.ReceiptNumber = numberRepository.GetNextReceiptNumber();
                            receipt.Date = DateTime.Now;
                            receipt.Sale = sale;
                            receipt.Total = total;

                            IReceiptRepository receiptRepository = new ReceiptRepository();
                            receiptRepository.SaveReceipt(receipt);

                            Console.WriteLine("Försäljning slutförd!");
                            Console.WriteLine($"Totalt: {total:F2} kr");
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Försäljningen kunde inte slutföras eftersom kvittot inte kunde sparas.");
                            break;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("Försäljningen kunde inte slutföras eftersom åtkomst till kvittofilen nekades.");
                            break;
                        }

                        Console.WriteLine();
                        Console.WriteLine("========== KVITTO ==========");
                        Console.WriteLine($"Kvittonummer: {receipt.ReceiptNumber}");
                        Console.WriteLine($"Datum: {receipt.Date:yyyy-MM-dd HH:mm:ss}");
                        Console.WriteLine("----------------------------");

                        foreach (SaleProducts item in receipt.Sale.SaleItems)
                        {
                            double price = item.UnitPrice * item.Quantity;

                            Console.WriteLine($"{item.Product.Name}");
                            Console.WriteLine($"  {item.Quantity} {item.Product.PriceType} x {item.UnitPrice:F2} kr = {price:F2} kr");
                        }

                        Console.WriteLine("----------------------------");
                        Console.WriteLine($"TOTALT: {receipt.Total:F2} kr");
                        Console.WriteLine("============================");
                        break;

                    case "2":
                        AdminService adminService = new AdminService();
                        adminService.ShowAdminMenu(products, campaignRepository);
                        break;

                    case "0":
                        Console.WriteLine("Välkommen åter!");
                        return;

                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
            }
        }
    }
}

