using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class CVConfiguration : IEntityTypeConfiguration<CV>
{
    public void Configure(EntityTypeBuilder<CV> builder)
    {
        builder.HasIndex(c => new { c.UserId, c.PositionId })
            .IsUnique();

        builder.Property(c => c.Version).IsConcurrencyToken();

        builder.HasOne(c => c.User)
            .WithMany(u => u.CVs)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Position)
            .WithMany(p => p.CVs)
            .HasForeignKey(c => c.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}