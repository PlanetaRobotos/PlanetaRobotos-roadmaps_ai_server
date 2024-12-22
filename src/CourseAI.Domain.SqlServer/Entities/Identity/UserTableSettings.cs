using CourseAI.Core.Constants;
using CourseAI.Core.Enums;
using CourseAI.Domain.Extensions;
using CourseAI.Domain.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Identity;

public class UserTableSettings : IEntity
{
    public long UserId { get; set; }
    public TableSettingsName TableName { get; set; }
    public string[] Columns { get; set; } = null!;
    
    public User? User { get; set; }


    internal class Configuration : IEntityTypeConfiguration<UserTableSettings>
    {
        public void Configure(EntityTypeBuilder<UserTableSettings> builder)
        {
            builder.ToTable($"{nameof(UserTableSettings)}").HasKey(e => new { e.UserId, e.TableName });

            builder.Property(e => e.TableName).IsRequired().HasMaxLength(StringLimits._100).HasEnumToStringConversion();
            builder.Property(e => e.Columns).IsRequired().HasColumnType("nvarchar(max)").HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            ).Metadata.SetValueComparer(new ArrayValueComparer<string>());
        }
    }
}