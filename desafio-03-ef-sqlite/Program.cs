using desafio_03_ef_sqlite.Data;
using desafio_03_ef_sqlite.Domain;
using desafio_03_ef_sqlite.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedDatabase();
}

app.MapGet("/products", async (AppDbContext db) => Results.Ok((object?)await db.Products.AsNoTracking().ToListAsync()));

app.MapGet("/products/{id:guid}", async (AppDbContext db, Guid id) =>
{
   var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
   return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", async (AppDbContext db, ProductRequest product) =>
{
    if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length > 100)
        return Results.BadRequest("Name is required and max 100 chars");

    if (product.Price <= 0)
        return Results.BadRequest("Price must be positive");

    if (product.QuantityInStock < 0)
        return Results.BadRequest("Quantity cannot be negative");

    
    var newProduct = new Product(product.Name, product.Price, product.QuantityInStock);
    db.Products.Add(newProduct);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{newProduct.Id}", newProduct);
});

app.MapPut("/products/{id:guid}", async (AppDbContext db, Guid id, ProductRequest product) =>
{
    var productInDb = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
    
    if (productInDb is null)
        return Results.NotFound(new { message = "Product not found"});
    
    if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length > 100)
        return Results.BadRequest("Name is required and max 100 chars");

    if (product.Price <= 0)
        return Results.BadRequest("Price must be positive");

    if (product.QuantityInStock < 0)
        return Results.BadRequest("Quantity cannot be negative");

    
    productInDb.Name = product.Name;
    productInDb.Price = product.Price;
    productInDb.QuantityInStock = product.QuantityInStock;
    await db.SaveChangesAsync();
    return Results.NoContent();
    
});

app.MapDelete("/products/{id:guid}", async (AppDbContext db, Guid id) =>
{
    var lines = await db.Products.Where(p => p.Id == id).ExecuteDeleteAsync();
    
    return lines == 0 ? Results.NotFound(new { message = "Product not found"}) : Results.NoContent();
});

app.Run();
