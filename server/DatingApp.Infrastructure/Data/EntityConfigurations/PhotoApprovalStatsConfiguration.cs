using DatingApp.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Data.EntityConfigurations;

public class PhotoApprovalStatsConfiguration : IEntityTypeConfiguration<PhotoApprovalStatsDto>
{
    public void Configure(EntityTypeBuilder<PhotoApprovalStatsDto> builder)
    {
        builder.HasNoKey(); 
    }
}