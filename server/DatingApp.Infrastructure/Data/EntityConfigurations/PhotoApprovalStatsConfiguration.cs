using DatingApp.Application.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Data.EntityConfigurations;

public class PhotoApprovalStatsConfiguration : IEntityTypeConfiguration<PhotoApprovalStatsResponse>
{
    public void Configure(EntityTypeBuilder<PhotoApprovalStatsResponse> builder)
    {
        builder.HasNoKey(); 
    }
}