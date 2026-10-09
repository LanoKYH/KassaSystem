using System;

namespace KassaSystem
{
    public class Receipt
    {
        public DateTime Date { get; set; }
        public Sale Sale { get; set; }
        public double Total { get; set; }
        public int ReceiptNumber { get; set; }
    }
}
