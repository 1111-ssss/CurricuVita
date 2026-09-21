using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PositionAttributeConfiguration : IEntityTypeConfiguration<PositionAttribute>
{
    public void Configure(EntityTypeBuilder<PositionAttribute> builder)
    {
        builder.HasKey(pa => new { pa.PositionId, pa.AttributeDefinitionId });

        builder.HasOne(pa => pa.Position)
            .WithMany(p => p.RequiredAttributes)
            .HasForeignKey(pa => pa.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pa => pa.AttributeDefinition)
            .WithMany(a => a.PositionAttributes)
            .HasForeignKey(pa => pa.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}