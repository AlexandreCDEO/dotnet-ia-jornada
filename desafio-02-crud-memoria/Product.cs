namespace desafio_02_crud_memoria;

public class Product(string name, decimal price, int quantity)
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public int QuantityInStock { get; set; } = quantity;
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
}