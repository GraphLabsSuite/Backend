using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace GraphLabs.Backend.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", "log.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 1024 * 1024)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var webHost = CreateWebHostBuilder(args).Build();

                using (var scope = webHost.Services.CreateScope())
                using (var context = scope.ServiceProvider.GetRequiredService<GraphLabsContext>())
                {
                    await InitializeDb(context, new InitialData(new PasswordHashCalculator()));
                }
            
                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
        
        
        private static async Task InitializeDb(GraphLabsContext context, InitialData initialData)
        {
            await context.Database.MigrateAsync();
            
            if (!context.Subjects.Any())
            {
                foreach (var subject in initialData.GetSubjects())
                {
                    context.Subjects.Add(subject);
                }
                await context.SaveChangesAsync();
            }
            if (!context.TaskModules.Any())
            {
                foreach (var module in initialData.GetTaskModules(context.Subjects))
                {
                    context.TaskModules.Add(module);
                }
                await context.SaveChangesAsync();
            }
            if (!context.TaskVariants.Any())
            {
                foreach (var variant in initialData.GetTaskVariants(context.TaskModules))
                {
                    context.TaskVariants.Add(variant);
                }
                await context.SaveChangesAsync();
            }
            if (!context.Users.Any())
            {
                foreach (var user in initialData.GetUsers())
                {
                    context.Users.Add(user);
                }
                await context.SaveChangesAsync();
            }
        }
    }    
}