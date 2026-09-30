using System;
using System.Collections.Generic;
using System.Text;

namespace KassaSystem
{
    public class Receipt
    {
        public DateTime Date { get; set; }
        public Sale Sale { get; set; }
        public double Total { get; set; }
    }
}
