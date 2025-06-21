using DatingApp.Application.Contracts.Responses;
using DatingApp.Core.Entities;
using DatingApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data;

public class DatabaseContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<PhotoTag> PhotoTags { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // Procedures
    public DbSet<PhotoApprovalStatsResponse> PhotoApprovalStats { get; set; }
    public DbSet<UserWithoutMainPhotoResponse> UsersWithoutMainPhoto { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}