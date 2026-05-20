namespace desafio_03_ef_sqlite.Domain;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime CreatedAt { get; set; }

    protected Product() { }
    
    public Product(string name, decimal price, int quantityInStock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        QuantityInStock = quantityInStock;
        CreatedAt = DateTime.UtcNow;
    }
}