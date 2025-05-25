using DatingApp.Data;
using DatingApp.Helpers;
using DatingApp.Infrastructure.Interfaces.IRepository;
using DatingApp.Infrastructure.Interfaces.IServices;
using DatingApp.Infrastructure.Repository;
using DatingApp.Intefaces;
using DatingApp.Repository;
using DatingApp.Repository.Interfaces;
using DatingApp.Services;
using DatingApp.Services.Interfaces;
using DatingApp.Services.Services;
using DatingApp.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<ILikesService, LikesService>();
        services.AddScoped<IMessagesService, MessagesService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IProcedureService, ProcedureService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IPhotoTagRepository, PhotoTagRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<LogUserActivity>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddSignalR();
        services.AddSingleton<PresenceTracker>();

        return services;
    }
}
