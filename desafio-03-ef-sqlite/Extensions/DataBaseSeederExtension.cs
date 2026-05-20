using desafio_03_ef_sqlite.Data;
using desafio_03_ef_sqlite.Domain;
using Microsoft.EntityFrameworkCore;

namespace desafio_03_ef_sqlite.Extensions;

public static class DataBaseSeederExtension
{
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();
        
        if (context.Products.Any())
            return app;
        
        var products = new List<Product>
        {
            new("Laptop", 2500.00m, 5),
            new("Mouse", 50.00m, 20),
            new("Teclado", 150.00m, 15)
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        return app;
    }

}