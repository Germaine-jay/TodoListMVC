using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Security.Cryptography;
using TodoList.DAL.Entities;
using TodoList.DAL.Enums;

namespace TodoList.DAL.Database
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Todo> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email, "IX_UniqueEmail")
                .IsUnique();

            modelBuilder.Entity<Todo>()
                .Property(t => t.Title)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Todo>()
                .Property(t => t.Description)
                .HasMaxLength(1000);


            base.OnModelCreating(modelBuilder);
        }


    }


}
