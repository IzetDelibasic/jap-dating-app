
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Data.EntityConfigurations;

public class PhotoConfiguration
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasQueryFilter(x => x.IsApproved);
    }
}
