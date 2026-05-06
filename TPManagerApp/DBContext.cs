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
        }
    }
}
