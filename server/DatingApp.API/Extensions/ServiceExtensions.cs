using DatingApp.Extensions;

namespace DatingApp.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplicationServices(config);
        services.AddIdentityServices(config);
        services.AddSwaggerDocumentation();
        services.AddCorsPolicy();
        return services;
    }
}
