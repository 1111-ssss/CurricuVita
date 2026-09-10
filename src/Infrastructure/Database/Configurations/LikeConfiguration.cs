using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasIndex(l => new { l.CVId, l.RecruiterId })
            .IsUnique();
        builder.HasIndex(l => l.RecruiterId);

        builder.HasOne(l => l.CV)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Recruiter)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.RecruiterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}