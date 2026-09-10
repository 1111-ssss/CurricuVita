using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(250).IsRequired();
        builder.Property(p => p.DescriptionMarkdown).HasColumnType("text");
        builder.Property(p => p.Version).IsConcurrencyToken();

        builder.HasIndex(p => p.Title);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.UpdatedAt);

        builder.HasOne(p => p.CreatedBy)
            .WithMany()
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}