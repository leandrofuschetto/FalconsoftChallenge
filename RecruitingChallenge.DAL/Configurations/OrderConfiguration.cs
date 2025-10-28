using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(o => o.EntryDate)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(14,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasDefaultValue(OrderStatus.Pending);

            builder.HasOne(o => o.Client)
                 .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
