using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RadiantBank.Domain.Repository;
using RadiantBank.Infrastructure.RadiantBankDB;
using RadiantBank.Infrastructure.RadiantBankDB.Repositories;

namespace RadiantBank.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<RadiantBankDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "RadiantBankDB"));
        services.AddScoped<IRadiantBankDbContext>(provider => provider.GetRequiredService<RadiantBankDbContext>());
        services.AddScoped<RadiantBankDbContextInitializer>();
        ConfigureRepositories(services);
        return services;
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}