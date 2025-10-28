using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL
{
    public class OrderNowDbContext : DbContext
    {
        public OrderNowDbContext(DbContextOptions<OrderNowDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ClientEntity> Clients => Set<ClientEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
