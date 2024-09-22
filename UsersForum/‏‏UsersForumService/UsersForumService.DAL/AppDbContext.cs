using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

using UsersForumService.DAL.Entities;

namespace UsersForumService.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post>? Posts { get; set; }
        public DbSet<Response>? Responses { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defining primary keys explicitly
            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);  // Post primary key

            modelBuilder.Entity<Response>()
                .HasKey(r => r.Id);  // Response primary key

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);  // User primary key

            // Post -> User (One-to-Many: A user can create many posts)
            modelBuilder.Entity<Post>()
                 .Ignore(p => p.ResponsersIDs)
                .HasOne(p => p.User)  // Post has one User (creator)
                .WithMany(u => u.Posts)  // User has many Posts
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Avoid cascading deletes

            // Response -> Post (Many-to-One: A post can have many responses)
            modelBuilder.Entity<Response>()
                .HasOne(r => r.Post)  // Response belongs to one Post
                .WithMany(p => p.Responses)  // Post has many Responses
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Post is deleted

            // Response -> User (One-to-Many: A user can create many responses)
            modelBuilder.Entity<Response>()
                .HasOne(r => r.User)  // Response belongs to one User
                .WithMany(u => u.Responses)  // User has many Responses
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Avoid cascading deletes

            // Configuring the User entity using Fluent API
            modelBuilder.Entity<User>(entity =>
            {
                // Specify the primary key for User
                entity.HasKey(u => u.Id);

                // Make Name a required field with max length
                entity.Property(u => u.Name)
                      .HasMaxLength(100);  // Set max length for Name

                entity.Property(u => u.Email)
                                   .IsRequired()   // Email is required (non-null)
                                   .HasMaxLength(150);  // Set max length for Email
              
                entity.Property(u => u.Password)
                                      .IsRequired()   // Password is required (non-null)
                                      .HasMaxLength(100);  // Set max length for Password

                // One-to-Many relationship (User -> Posts)
                entity.HasMany(u => u.Posts)
                      .WithOne(p => p.User)  // A post has one User
                      .HasForeignKey(p => p.UserId)  // Foreign key for the relationship
                      .OnDelete(DeleteBehavior.Restrict);  // Disable cascading deletes
            });

            base.OnModelCreating(modelBuilder);
        }
    }
   
}
