using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PositionTagConfiguration : IEntityTypeConfiguration<PositionTag>
{
    public void Configure(EntityTypeBuilder<PositionTag> builder)
    {
        builder.HasIndex(pt => new { pt.PositionId, pt.TagId })
            .IsUnique();

        builder.HasOne(pt => pt.Position)
            .WithMany(p => p.RequiredTags)
            .HasForeignKey(pt => pt.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.Tag)
            .WithMany()
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
