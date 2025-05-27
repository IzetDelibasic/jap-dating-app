using System.Text.Json;
using DatingApp.Core.Entities;
using DatingApp.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager, IWebHostEnvironment env, DatabaseContext databaseContext)
    {
        if (await userManager.Users.AnyAsync()) return;

        if (!await databaseContext.Tags.AnyAsync())
        {
            var tags = new List<Tag>
        {
            new Tag { Name = "Travel" },
            new Tag { Name = "Music" },
            new Tag { Name = "Sports" },
            new Tag { Name = "Movies" },
            new Tag { Name = "Food" }
        };

            await databaseContext.Tags.AddRangeAsync(tags);
            await databaseContext.SaveChangesAsync();
        }

        // Question
        var userDataPath = Path.Combine(Directory.GetCurrentDirectory(), "../DatingApp.Infrastructure/Data/UserSeedData.json");

        var userData = await File.ReadAllTextAsync(userDataPath);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        var roles = new List<AppRole>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.Photos.First().IsApproved = true;

            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

        }

        var admin = new AppUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",
            City = "",
            Country = "",
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
}
