using DatingApp.Extensions;
using DatingApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomMiddlewares(app.Environment);

app.MapControllers();
app.MapSignalRHubs();

await app.InitializeDatabaseAsync();

app.Run();