using DatingApp.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Data.EntityConfigurations;

public class UserWithoutMainPhotoConfiguration : IEntityTypeConfiguration<UserWithoutMainPhotoDto>
{
    public void Configure(EntityTypeBuilder<UserWithoutMainPhotoDto> builder)
    {
        builder.HasNoKey();
    }
}