using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace RadiantBank.API;

public static class ConfigureServices
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        //Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        return services;
    }
}