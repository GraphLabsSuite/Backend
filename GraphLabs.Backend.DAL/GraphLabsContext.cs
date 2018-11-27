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
            modelBuilder.Entity<TaskVariant>().HasOne(v => v.TaskModule);
        }
    }
}