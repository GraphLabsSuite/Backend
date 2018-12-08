using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraphLabs.Backend.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<GraphLabsContext>())
            {
                await InitializeDb(context, new InitialData());
            }
            
            await webHost.RunAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        
        
        private static async Task InitializeDb(GraphLabsContext context, InitialData initialData)
        {
            await context.Database.MigrateAsync();
                
            if (!context.TaskModules.Any())
            {
                foreach (var module in initialData.GetTaskModules())
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