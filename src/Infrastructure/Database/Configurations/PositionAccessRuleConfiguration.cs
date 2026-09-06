using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PositionAccessRuleConfiguration : IEntityTypeConfiguration<PositionAccessRule>
{
    public void Configure(EntityTypeBuilder<PositionAccessRule> builder)
    {
        builder.Property(r => r.Operator).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Value).HasMaxLength(200).IsRequired();

        builder.HasOne(r => r.Position)
            .WithMany(p => p.AccessRules)
            .HasForeignKey(r => r.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.AttributeDefinition)
            .WithMany()
            .HasForeignKey(r => r.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}