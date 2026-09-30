
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string PriceType { get; set; }
    public bool IsAvailable { get; private set; }

    public Product()
        {
        IsAvailable = true;

        }

    public void MakeUnvailable()

    {
        IsAvailable = false;
    }
}

