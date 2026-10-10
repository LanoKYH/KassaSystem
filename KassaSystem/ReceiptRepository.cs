using System;
using System.IO;

namespace KassaSystem
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly string fileName;

        public ReceiptRepository(string fileName = null)
        {
            this.fileName = fileName;
        }
        public void SaveReceipt(Receipt receipt)
        {
            string receiptFileName = fileName
                ?? $"RECEIPT_{receipt.Date:yyyyMMdd}.txt";

            string receiptText = "===== KVITTO =====\n";
            receiptText += $"Kvittonummer: {receipt.ReceiptNumber}\n";
            receiptText += $"Datum: {receipt.Date}\n";

            foreach (SaleProducts item in receipt.Sale.SaleItems)
            {
                double price = item.UnitPrice * item.Quantity;

                receiptText +=
                    $"{item.Product.Name} - {item.Quantity} " +
                    $"{item.Product.PriceType} x {item.UnitPrice} kr = " +
                    $"{price} kr\n";
            }

            receiptText += $"TOTALT: {receipt.Total} kr\n";
            receiptText += "==================\n";

            try
            {
                File.AppendAllText(receiptFileName, receiptText);
                Console.WriteLine("Kvitto sparat.");
            }
            catch (IOException)
            {
                Console.WriteLine("Kunde inte spara kvittot.");
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Åtkomst nekad när kvittot skulle sparas.");
                throw;
            }
        }
    }
}
