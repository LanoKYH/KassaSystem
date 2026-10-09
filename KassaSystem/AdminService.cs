using System;
using System.Collections.Generic;

namespace KassaSystem
{
    public class AdminService
    {
        public void ShowAdminMenu(List<Product> products, CampaignRepository campaignRepository)
        {
            Console.WriteLine();
            Console.WriteLine("=== ADMIN PANEL ===");
            Console.WriteLine("1. Lägg till produkt");
            Console.WriteLine("2. Ändra produkt");
            Console.WriteLine("3. Lägg till kampanj");
            Console.WriteLine("4. Ta bort kampanj");
            Console.WriteLine("0. Tillbaka");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                   
                    Console.WriteLine("Ange produkt ID:");
                    string newProductId = Console.ReadLine();

                    Product existingProduct = products.Find(p => p.Id == newProductId);

                    if (existingProduct != null)
                    {
                        Console.WriteLine("Produkt-ID används redan.");
                        break;
                    }

                    Console.WriteLine("Ange produktnamn:");
                    string newProductName = Console.ReadLine();

                    Console.WriteLine("Ange pris:");
                    if (!double.TryParse(Console.ReadLine(), out double newProductPrice))
                    {
                        Console.WriteLine("Priset måste vara ett nummer.");
                        break;
                    }

                    if (newProductPrice <= 0)
                    {
                        Console.WriteLine("Priset måste vara större än 0.");
                        break;
                    }

                    Console.WriteLine("Ange pristyp (Styck eller kg):");
                    string newPriceType = Console.ReadLine();

                    if (newPriceType != "Styck" && newPriceType != "kg")
                    {
                        Console.WriteLine("Pristyp måste vara Styck eller kg.");
                        break;
                    }

                    Product newProduct = new Product();

                    newProduct.Id = newProductId;
                    newProduct.Name = newProductName;
                    newProduct.Price = newProductPrice;
                    newProduct.PriceType = newPriceType;

                    products.Add(newProduct);

                    ProductRepository productRepository = new ProductRepository();
                    productRepository.SaveProducts(products);

                    Console.WriteLine("Produkten har lagts till.");

                    break;

                case "2":
                    Console.WriteLine("Ange produkt ID för produkten du vill ändra:");
                    string productIdToChange = Console.ReadLine();

                    Product productToChange = products.Find(p => p.Id == productIdToChange);

                    if (productToChange == null)
                    {
                        Console.WriteLine("Produkten finns inte.");
                        break;
                    }

                    Console.WriteLine("Ange nytt produktnamn:");
                    string newName = Console.ReadLine();

                    productToChange.Name = newName;

                    Console.WriteLine("Ange nytt pris:");

                    if (!double.TryParse(Console.ReadLine(), out double newPrice))
                    {
                        Console.WriteLine("Priset måste vara ett nummer.");
                        break;
                    }

                    if (newPrice <= 0)
                    {
                        Console.WriteLine("Priset måste vara större än 0.");
                        break;
                    }

                    productToChange.Price = newPrice;

                    ProductRepository repository = new ProductRepository();
                    repository.SaveProducts(products);

                    Console.WriteLine("Produkten har uppdaterats.");

                    break;


                case "3":
                    Console.WriteLine("Ange produkt ID:");
                    string productId = Console.ReadLine();

                    Product product = products.Find(p => p.Id == productId);

                    if (product == null)
                    {
                        Console.WriteLine("Produkten finns inte.");
                        break;
                    }
                   
                    Console.WriteLine("Ange nya kampanjpris:");
                    if (!double.TryParse(Console.ReadLine(), out double campaignPrice))
                    {
                        Console.WriteLine("Kampanjpriset måste vara ett nummer.");
                        break;
                    }

                    if (campaignPrice <= 0)
                    {
                        Console.WriteLine("Kampanjpriset måste vara större än 0.");
                        break;
                    }

                    Console.WriteLine("Ange startdatum (yyyy-MM-dd):");

                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Ogiltigt datum.");
                        break;
                    }

                    Console.WriteLine("Ange slutdatum (yyyy-MM-dd):");

                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        Console.WriteLine("Ogiltigt datum.");
                        break;
                    }

                    if (endDate < startDate)
                    {
                        Console.WriteLine("Slutdatum kan inte vara före startdatum.");
                        break;
                    }

                    Campaign campaign = new Campaign();

                    campaign.Product = product;
                    campaign.CampaignPrice = campaignPrice;
                    campaign.StartDate = startDate;
                    campaign.EndDate = endDate;

                    campaignRepository.AddCampaign(campaign);
                    campaignRepository.SaveCampaigns();
                    Console.WriteLine("Kampanj skapad!");

                    break;

                case "4":
                    Console.WriteLine("Ange produkt-ID för kampanjen:");
                    string productIdToRemove = Console.ReadLine();

                    Product productToRemove = products.Find(p => p.Id == productIdToRemove);

                    if (productToRemove == null)
                    {
                        Console.WriteLine("Produkten finns inte.");
                        break;
                    }

                    Campaign campaignToRemove = campaignRepository.GetActiveCampaign(productToRemove);

                    if (campaignToRemove == null)
                    {
                        Console.WriteLine("Produkten har ingen aktiv kampanj.");
                        break;
                    }

                    campaignRepository.RemoveCampaign(campaignToRemove);
                    campaignRepository.SaveCampaigns();
                    Console.WriteLine("Kampanjen har tagits bort.");

                    break;

                case "0":
                    Console.WriteLine();
                    Console.WriteLine("Återgår till huvudmenyn..");
                    Console.WriteLine();
                    return;

                default:
                    Console.WriteLine("Ogiltigt val.");
                    return;
            }
        }
    }
}
