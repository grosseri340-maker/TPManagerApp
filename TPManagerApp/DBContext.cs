using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TPManagerApp
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"workstation id=TPManager.mssql.somee.com;packet size=4096;user id=AntonTren_SQLLogin_1;pwd=963cdp5k2g;data source=TPManager.mssql.somee.com;persist security info=False;initial catalog=TPManager;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreditCards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CreditCard>()
                .HasMany(c => c.Operations)
                .WithOne(o => o.CreditCard)
                .HasForeignKey(o => o.CreditCardId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Operations)
                .WithOne(o => o.Category)
                .HasForeignKey(o => o.CategoryId);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Login)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Password)
                      .IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Продукти" },
                 new Category { Id = 2, Name = "Транспорт" },
                 new Category { Id = 3, Name = "Розваги" }
            );

            modelBuilder.Entity<CreditCard>().HasData(
                new CreditCard
                {
                    Id = 1,
                    CardNumber = 1234567890123456,
                    CardType = "Visa",
                    Cash = 1000.00m,
                    UserId = 1
                },
                new CreditCard
                {
                    Id = 2,
                    CardNumber = 9876543210987654,
                    CardType = "MasterCard",
                    Cash = 500.50m,
                    UserId = 1
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "LilSneako",
                    Login = "sheisty145",
                    Password = "ybuuf16" 
                }
            );

            modelBuilder.Entity<Operation>().HasData(
                new Operation
                {
                    Id = 1,
                    Name = "Grocery Shopping",
                    CashAmount = 1250.50m,
                    Date = new DateTime(2026, 5, 10),
                    CreditCardId = 1,
                    CategoryId = 1
                },
                new Operation
                {
                    Id = 2,
                    Name = "Netflix Subscription",
                    CashAmount = 299.99m,
                    Date = new DateTime(2026, 5, 11),
                    CreditCardId = 1,
                    CategoryId = 3
                },
                new Operation
                {
                    Id = 3,
                    Name = "Fuel Payment",
                    CashAmount = 1800.00m,
                    Date = new DateTime(2026, 5, 12),
                    CreditCardId = 1,
                    CategoryId = 2
                }
            );
        }
    }
}
