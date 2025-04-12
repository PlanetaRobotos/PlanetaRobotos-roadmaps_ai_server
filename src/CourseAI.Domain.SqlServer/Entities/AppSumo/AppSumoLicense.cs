using CourseAI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.AppSumo;

public class AppSumoLicense: IDateableEntity<Guid>
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    
    public string LicenseKey { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenExpiry { get; set; }
    public string PlanId { get; set; }
    public int Tier { get; set; } 
    public string Status { get; set; } // active, cancelled, refunded
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    
    public class AppSumoLicenseConfiguration : IEntityTypeConfiguration<AppSumoLicense>
    {
        public void Configure(EntityTypeBuilder<AppSumoLicense> builder)
        {
            builder.ToTable($"{nameof(AppSumoLicense)}s").HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            
            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<AppSumoLicense>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.LicenseKey)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AccessToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.RefreshToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        }
    }
}