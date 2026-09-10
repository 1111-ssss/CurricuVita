using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class DiscussionMessageConfiguration : IEntityTypeConfiguration<DiscussionMessage>
{
    public void Configure(EntityTypeBuilder<DiscussionMessage> builder)
    {
        builder.Property(m => m.ContentMarkdown).HasColumnType("text").IsRequired();

        builder.HasIndex(m => new { m.PositionId, m.CreatedAt });

        builder.HasOne(m => m.Position)
            .WithMany(p => p.Messages)
            .HasForeignKey(m => m.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Author)
            .WithMany()
            .HasForeignKey(m => m.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}