using DatingApp.Extensions;
using DatingApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCorsPolicy();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.MapControllers();
app.MapSignalRHubs();

await app.InitializeDatabaseAsync();

app.Run();