using System.IO;

namespace KassaSystem
{
    public class ReceiptNumberRepository
    {
        private string fileName = "receiptNumber.txt";

        public int GetNextReceiptNumber()
        {
            int number = 1000;

            if (File.Exists(fileName))
            {
                string savedNumber = File.ReadAllText(fileName);

                int.TryParse(savedNumber, out number);
            }

            number++;

            File.WriteAllText(fileName, number.ToString());

            return number;
        }
    }
}
