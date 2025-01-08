using CourseAI.Core.Enums;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Common;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using CourseAI.Domain.Entities.Transactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.Data.EntityFramework.Extensions;

#pragma warning disable CS8618

namespace CourseAI.Domain.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    public DbSet<Resource> Resources { get; init; }

    public DbSet<UserTableSettings> TableSettings { get; init; }

    public DbSet<Roadmap> Roadmaps { get; init; }
    public DbSet<RoadmapModule> RoadmapModules { get; init; }
    public DbSet<Lesson> Lessons { get; init; }
    public DbSet<Quiz> Quizzes { get; init; }
    
    public DbSet<UserRoadmap> UserRoadmaps { get; init; } 
    public DbSet<UserQuiz> UserQuizzes { get; set; }
    public DbSet<UserLike> UserLikes { get; set; }
    public DbSet<TokenTransaction> TokenTransactions { get; set; }
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<UserPurchase> UserPurchases { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ConfigureEntities(config =>
        {
            config.EngineStrategy = DbEngineStrategy.SqlServer;
            config.SequentialGuids = true;
            config.ApplyEntityIds = true;
            config.ApplyEntityDating = true;
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