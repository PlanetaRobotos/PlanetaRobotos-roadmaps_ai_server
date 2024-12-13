using CourseAI.Core.Enums;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Common;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.Data.EntityFramework.Extensions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace CourseAI.Domain.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    // R
    public DbSet<Resource> Resources { get; init; }

    // U
    public DbSet<UserTableSettings> TableSettings { get; init; }
    
    // R
    public DbSet<Roadmap> Roadmaps { get; init; }
    public DbSet<RoadmapModule> RoadmapModules { get; init; }
    public DbSet<Lesson> Lessons { get; init; }
    
    public DbSet<UserRoadmap> UserRoadmaps { get; init; } 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureEntities(config =>
        {
            config.EngineStrategy = DbEngineStrategy.SqlServer;
            // Use orderable GUIDs
            config.SequentialGuids = true;
            // Auto-populate IDs
            config.ApplyEntityIds = true;
            // Auto-populate Created and Updated fields
            config.ApplyEntityDating = true;
            // Apply data seeders
            config.ApplyDataSeeders = true;
            config.ApplyEntityTypeConfigurations = true;
            config.DateTimeKind = DateTimeKind.Utc;
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.ApplyUtcDateTimeConversions();

        configurationBuilder.Properties<ContactType>().HaveConversion<EnumToStringConverter<ContactType>>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableDetailedErrors();
    }
}