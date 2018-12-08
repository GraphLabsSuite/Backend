using System;
using GraphLabs.Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace GraphLabs.Backend.DAL
{
    public class GraphLabsContext : DbContext
    {
        public GraphLabsContext(DbContextOptions<GraphLabsContext> options)
            : base(options)
        {
        }

        public DbSet<TaskModule> TaskModules { get; protected set; }
        public DbSet<TaskVariant> TaskVariants { get; protected set; }
        public DbSet<User> Users { get; protected set; }
        public DbSet<Student> Students { get; protected set; }
        public DbSet<Teacher> Teachers { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var taskVariant = modelBuilder.Entity<TaskVariant>();
            taskVariant
                .HasKey(v => v.Id);
            taskVariant
                .HasOne(v => v.TaskModule)
                .WithMany(t => t.Variants)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            var taskModule = modelBuilder.Entity<TaskModule>();
            taskModule.HasKey(t => t.Id);

            var user = modelBuilder.Entity<User>();
            user.HasKey(u => u.Id);
            user.Property(u => u.Email).IsRequired();
            user.HasIndex(u => u.Email).IsUnique();
            
            var student = modelBuilder.Entity<Student>();
            student.HasIndex(s => s.Group);
            
            modelBuilder.Entity<Teacher>();
        }
    }
}