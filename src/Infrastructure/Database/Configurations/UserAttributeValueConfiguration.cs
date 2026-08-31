using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class UserAttributeValueConfiguration : IEntityTypeConfiguration<UserAttributeValue>
{
    public void Configure(EntityTypeBuilder<UserAttributeValue> builder)
    {
        builder.HasIndex(v => new { v.UserId, v.AttributeDefinitionId })
            .IsUnique();

        builder.Property(v => v.Version).IsConcurrencyToken();

        builder.HasOne(v => v.User)
            .WithMany(u => u.AttributeValues)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.AttributeDefinition)
            .WithMany(a => a.UserValues)
            .HasForeignKey(v => v.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}