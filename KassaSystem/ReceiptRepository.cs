using System;
using System.IO;

namespace KassaSystem
{
    public class ReceiptRepository
    {
        public void SaveReceipt(Receipt receipt)
        {
            string fileName = $"RECEIPT_{receipt.Date:yyyyMMdd}.txt";

            string receiptText = $"===== KVITTO =====\n";
            receiptText += $"Datum: {receipt.Date}\n";

            foreach (SaleProducts item in receipt.Sale.SaleItems)
            {
                double price = item.Product.Price * item.Quantity;

                receiptText += $"{item.Product.Name} - {item.Quantity} {item.Product.PriceType} x {item.Product.Price} kr = {price} kr\n";
            }

            receiptText += $"TOTALT: {receipt.Total} kr\n";
            receiptText += "==================\n";

            File.AppendAllText(fileName, receiptText);
        }
    }
}
