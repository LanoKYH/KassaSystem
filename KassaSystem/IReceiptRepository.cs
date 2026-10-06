using System;
using System.Collections.Generic;
using System.Text;

namespace KassaSystem
{
    public interface IReceiptRepository
    {
        void SaveReceipt(Receipt receipt);
    }
}
