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
        public DbSet<TaskVariantLog> TaskVariantLogs { get; protected set; }

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
            user.Property(u => u.FirstName).IsRequired();
            user.Property(u => u.LastName).IsRequired();
            user.Property(u => u.PasswordHash).IsRequired();
            user.Property(u => u.PasswordSalt).IsRequired();
            
            var student = modelBuilder.Entity<Student>();
            student.HasIndex(s => s.Group);
            
            modelBuilder.Entity<Teacher>();

            var log = modelBuilder.Entity<TaskVariantLog>();
            log.HasKey(l => l.Id);
            log.Property(l => l.Action)
                .IsRequired();
            log.HasOne(l => l.Variant)
                .WithMany(v => v.Logs)
                .IsRequired()
                .HasForeignKey(l => l.VariantId)
                .OnDelete(DeleteBehavior.Restrict);
            log.HasOne(l => l.Student)
                .WithMany(s => s.Logs)
                .IsRequired()
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            log.HasIndex(l => l.DateTime);
        }
    }
}