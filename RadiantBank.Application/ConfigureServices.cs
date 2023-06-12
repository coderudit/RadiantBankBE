using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RadiantBank.Application.Common.Behaviours;
using RadiantBank.Application.Services.Implementations;
using RadiantBank.Application.Services.Interfaces;

namespace RadiantBank.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}