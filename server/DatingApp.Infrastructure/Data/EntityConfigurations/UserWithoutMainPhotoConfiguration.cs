using DatingApp.Application.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Data.EntityConfigurations;

public class UserWithoutMainPhotoConfiguration : IEntityTypeConfiguration<UserWithoutMainPhotoResponse>
{
    public void Configure(EntityTypeBuilder<UserWithoutMainPhotoResponse> builder)
    {
        builder.HasNoKey();
    }
}