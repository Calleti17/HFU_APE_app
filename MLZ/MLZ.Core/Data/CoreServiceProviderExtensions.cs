using Microsoft.Extensions.DependencyInjection;
using MLZ.Core.Services;
using MLZ.Core.Models;
using MLZ.ViewModels;

namespace Core.Services;

public static class CoreServiceProviderExtensions
{
    public static IServiceCollection CreateDefaultServiceCollection()
    {
        return new ServiceCollection().AddDefaultServices();
    }

    public static IServiceCollection AddDefaultServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ILocalStorage,SqliteLocalStorage>()
            .AddSingleton<LocalStorageSettings>()
            .AddSingleton<Fisch>()
            .AddTransient<FischViewModel>();
    }

    public static IServiceProvider CreateDefaultServiceProvider()
    {
        return CreateDefaultServiceCollection().BuildServiceProvider();
    }
}