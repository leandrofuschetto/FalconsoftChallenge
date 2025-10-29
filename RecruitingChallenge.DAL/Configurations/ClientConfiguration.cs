using Microsoft.EntityFrameworkCore;
using RecruitingChallenge.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ClientEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(u => u.Orders)
               .WithOne(p => p.Client)
               .HasForeignKey(p => p.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Email);
        }
    }
}
