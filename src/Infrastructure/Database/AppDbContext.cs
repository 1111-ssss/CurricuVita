using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }
    public DbSet<Badge> Badges { get; set; }
    public DbSet<CV> CVs { get; set; }
    public DbSet<DiscussionMessage> DiscussionMessages { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<PositionAccessRule> PositionAccessRules { get; set; }
    public DbSet<PositionAttribute> PositionAttributes { get; set; }
    public DbSet<PositionTag> PositionTags { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTag> ProjectTags { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserAttributeValue> UserAttributeValues { get; set; }
    public DbSet<UserBadge> UserBadges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}