using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitingChallenge.DAL.Entities;

namespace RecruitingChallenge.DAL.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(p => p.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(14,2)");

            builder.Property(p => p.EntryDate)
                .IsRequired();                
        }
    }
}
