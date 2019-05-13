using System;
using System.Data;
using System.Data.Common;
using GraphLabs.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GraphLabs.Backend.DAL
{
    public class GraphLabsContext : DbContext
    {
        private readonly Lazy<IUserInfoService> _userInfoService;

        public GraphLabsContext(DbContextOptions<GraphLabsContext> options)
            : base(options)
        {
            _userInfoService = new Lazy<IUserInfoService>(this.GetService<IUserInfoService>);
        }

        private DbConnection _dbConnection;
        public override DatabaseFacade Database 
        {
            get
            {
                var db = base.Database;
                if (_dbConnection == null)
                {
                    _dbConnection = db.GetDbConnection();
                    _dbConnection.StateChange += DbConnectionOnStateChange; 
                }

                return db;
            }
        }

        private void DbConnectionOnStateChange(object sender, StateChangeEventArgs e)
        {
            if (e.OriginalState == ConnectionState.Closed && e.CurrentState == ConnectionState.Open)
            {
                var enableRls = _userInfoService.Value.UserRole == nameof(Student) ? 1 : 0;
                using (var cmd = _dbConnection.CreateCommand())
                {
                    cmd.CommandText = $"set backend.enableRls = {enableRls};";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = _dbConnection.CreateCommand())
                {
                    cmd.CommandText = $"set backend.userId = '{_userInfoService.Value.UserId}';";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
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

        public override void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.StateChange -= DbConnectionOnStateChange;
                _dbConnection = null;
            }

            base.Dispose();
        }
    }
}