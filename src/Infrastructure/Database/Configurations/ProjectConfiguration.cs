using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.DescriptionMarkdown).HasColumnType("text");
        builder.Property(p => p.Version).IsConcurrencyToken();

        builder.HasIndex(p => new { p.UserId, p.StartDate });
        builder.HasIndex(p => new { p.UserId, p.EndDate });
        builder.HasIndex(p => p.Title);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}