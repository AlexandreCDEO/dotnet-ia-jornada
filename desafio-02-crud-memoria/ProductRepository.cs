using System.Collections.Concurrent;

namespace desafio_02_crud_memoria;

public static class ProductRepository
{
    private static ConcurrentDictionary<Guid, Product> Products { get; set; } = [];

    public static List<Product> GetProducts() {
        return Products.Values.ToList();
    }

    public static Product? GetProduct(Guid id)
    {
        var existsProduct = Products.TryGetValue(id, out var product);
        return existsProduct ? product : null;
    }
    
    public static bool AddProduct(Product product)
    {
       return Products.TryAdd(product.Id, product); 
    }

    public static bool UpdateProduct(Guid id, Product product)
    {
        if (!Products.ContainsKey(id)) return false;
        
        Products[id] = product;
        return true;
    }
    
    public static bool RemoveProduct(Guid id) {
       return Products.TryRemove(id, out _);
    } 
}