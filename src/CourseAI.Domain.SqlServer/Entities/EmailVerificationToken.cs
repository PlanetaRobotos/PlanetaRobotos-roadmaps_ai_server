using System.Security.Principal;
using CourseAI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities;

public class EmailVerificationToken
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public User User { get; set; }
    
    internal sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
        {
            builder.ToTable($"{nameof(EmailVerificationToken)}s").HasKey(x => x.Id); 
            
            builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        }
    }
}