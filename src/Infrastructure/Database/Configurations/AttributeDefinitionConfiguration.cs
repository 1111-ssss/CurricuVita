using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class AttributeDefinitionConfiguration : IEntityTypeConfiguration<AttributeDefinition>
{
    public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
    {
        builder.HasIndex(a => a.Name).IsUnique();

        builder.Property(a => a.Name).HasMaxLength(150).IsRequired();
        builder.Property(a => a.Category).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Description).HasMaxLength(1000);
        builder.Property(a => a.DataType).IsRequired();
        builder.Property(a => a.OptionsJson).HasColumnType("text");
    }
}