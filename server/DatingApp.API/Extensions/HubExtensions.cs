using DatingApp.SignalR;

namespace DatingApp.Extensions
{
    public static class HubExtensions
    {
        public static IEndpointRouteBuilder MapSignalRHubs(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<PresenceHub>("hubs/presence");
            endpoints.MapHub<MessageHub>("hubs/messages");

            return endpoints;
        }
    }
}