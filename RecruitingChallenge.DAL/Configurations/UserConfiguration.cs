using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitingChallenge.DAL.Entities;

namespace RecruitingChallenge.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.EntryDate)
               .IsRequired();
        }
    }
}
