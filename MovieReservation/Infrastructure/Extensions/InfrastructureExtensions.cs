using System.ComponentModel.Design;
using Domain.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services , IConfiguration configuration)
    {
        
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        
        
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });


        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}