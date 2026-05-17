var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "v1"); 
        options.RoutePrefix = "swagger";
    });
}

app.MapGet("/api/saude", () => new
{
    status = "OK",
    versao = "1.0",
    timestamp = DateTime.UtcNow.ToString("o")
})
.WithName("GetHealth")
.WithDescription("Verifica se a API está saudável");

app.Run();
