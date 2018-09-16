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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // todo: setup entity relations
        }
    }
}