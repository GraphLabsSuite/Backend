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

        public DbSet<TaskModule> TaskModules { get; set; }
        public DbSet<TaskVariant> TaskVariants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var taskVariant = modelBuilder.Entity<TaskVariant>();
            taskVariant
                .HasKey(v => v.Id);
            taskVariant
                .HasOne(v => v.TaskModule)
                .WithMany(t => t.Variants)
                .IsRequired();

            var taskModule = modelBuilder.Entity<TaskModule>();
            taskModule.HasKey(t => t.Id);
        }
    }
}