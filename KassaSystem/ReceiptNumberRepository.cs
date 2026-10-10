using System;
using System.IO;

namespace KassaSystem
{
    public class ReceiptNumberRepository
    {
        private readonly string fileName;

        public ReceiptNumberRepository(string fileName = "receiptNumber.txt")
        {
            this.fileName = fileName;
        }
        public int GetNextReceiptNumber()
        {
            int number = 1000;

            try
            {
                if (File.Exists(fileName))
                {
                    string savedNumber = File.ReadAllText(fileName);

                    if (!int.TryParse(savedNumber, out number) || number < 1000)
                    {
                        throw new InvalidDataException(
                            "Det sparade kvittonumret är ogiltigt.");
                    }
                }

                if (number == int.MaxValue)
                {
                    throw new InvalidOperationException(
                        "Det går inte att skapa fler kvittonummer.");
                }

                number++;

                File.WriteAllText(fileName, number.ToString());

                return number;
            }
            catch (IOException)
            {
                Console.WriteLine("Kunde inte läsa eller spara kvittonumret.");
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Åtkomst nekad till kvittonummerfilen.");
                throw;
            }
        }
    }
}
