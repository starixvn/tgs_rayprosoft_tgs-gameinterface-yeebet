using System.Reflection;
using Tgs.GameInterface.YeeBet.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceCore(
        this IServiceCollection services,
        Action<IPAddressOptions, IServiceProvider> ipAddressOptions)
        => services
            .AddOptions<IPAddressOptions>()
            .Configure(ipAddressOptions)
            .Services
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddSingleton<ISnowflakeUtensil, SnowflakeUtensil>()
            .AddSingleton<IYeeBetHasher, YeeBetHasher>();
}
