using desafio_03_ef_sqlite.Data;
using Microsoft.EntityFrameworkCore;

namespace desafio_03_ef_sqlite.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("Connection string not found");

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite(connectionString);
        });
        
        return services;

    }
}