using desafio_02_crud_memoria;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/health", () => new
{
    status = "OK",
    version = "1.00",
    timestamp = DateTime.UtcNow.ToString("o"),
});

app.MapGet("/api/products", () => Results.Ok(ProductRepository.GetProducts()));

app.MapGet("/api/products/{id:guid}", (Guid id) =>
{
    var product = ProductRepository.GetProduct(id);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/api/products", (CreateProductCommand product) =>
{
    var newProduct = new Product(product.Name, product.Price, product.QuantityInStock);
    return !ProductRepository.AddProduct(newProduct) ? Results.BadRequest() : Results.Created($"/api/products/{newProduct.Id}", newProduct);
}).AddEndpointFilter<ValidationFilter<CreateProductCommand>>();

app.MapPut("/api/products/{id:guid}", (Guid id, CreateProductCommand product) =>
{
    var existingProduct = ProductRepository.GetProduct(id);
    if (existingProduct is null) return Results.NotFound();
    existingProduct.Name = product.Name;
    existingProduct.Price = product.Price;
    existingProduct.QuantityInStock = product.QuantityInStock;
    ProductRepository.UpdateProduct(id, existingProduct);
    return Results.NoContent();
    
}).AddEndpointFilter<ValidationFilter<CreateProductCommand>>();

app.MapDelete("/api/products/{id:guid}", (Guid id) => !ProductRepository.RemoveProduct(id) ? Results.NotFound() :  Results.NoContent());

app.Run();
