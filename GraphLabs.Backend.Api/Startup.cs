using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using GraphLabs.Backend.Api.Auth;
using GraphLabs.Backend.Api.Controllers;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using ServiceLifetime = Microsoft.OData.ServiceLifetime;

namespace GraphLabs.Backend.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = GetType().Assembly.FullName;
            // var postgresHost = Environment.GetEnvironmentVariable("DB_HOST");
            // var postgresDb = Environment.GetEnvironmentVariable("DB_NAME");
            // var postgresUser = Environment.GetEnvironmentVariable("DB_USER");
            // var postgresPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            // var postgresPort = Environment.GetEnvironmentVariable("DB_PORT");
            // var postgresHost = "postgres.example.com";
            // var postgresDb = "graphlabs";
            // var postgresUser = "graphlabs";
            // var postgresPassword = "my_password";
            
            var postgresHost = "db";
            var postgresDb = "base6";
            var postgresUser = "postgres";
            var postgresPassword = "5432";
            var postgresPort = "5432";
            

            services.AddDbContext<GraphLabsContext>(
                o => o.UseNpgsql($"Host={postgresHost};Database={postgresDb};Username={postgresUser};Password={postgresPassword};Port={postgresPort}", b => b.MigrationsAssembly(migrationsAssembly)));
            services.AddSingleton<InitialData>();
            services.AddCors();

            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddOData();
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // configure strongly typed settings objects
            var authSettings = _configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettings);
            
            var appSettings = authSettings.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    if (_environment.IsDevelopment())
                        x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            services.AddScoped<UserService>();
            services.AddSingleton<PasswordHashCalculator>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<TaskVariantConverter>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Converters =
                {
                    new StringEnumConverter
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new LowerCamelCaseContractResolver()
            };
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging();
            
            app.UseAuthentication();
            
            app.UseMvc(builder =>
            {
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                const string routeName = "odata";
                builder.MapODataServiceRoute(routeName, routeName, routeBuilder => routeBuilder
                    .AddService(ServiceLifetime.Singleton, sp => GetEdmModel())
                    .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton,
                        sp => ODataRoutingConventions.CreateDefaultWithAttributeRouting(routeName, builder))
                );
            });
        }
        
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder
            {
                Namespace = "GraphLabs"
            };

            // TaskModules =============================================================================================
            var taskModule = builder.EntitySet<TaskModule>("TaskModules").EntityType;
            taskModule.HasKey(m => m.Id);
            taskModule.HasMany(m => m.Variants);
            
            taskModule.Function(nameof(TaskModulesController.RandomVariant)).Returns<IActionResult>();
            
            var downloadFunc = taskModule.Function(nameof(TaskModulesController.Download));
            downloadFunc.Parameter<string>("path").Required();
            downloadFunc.Returns<IActionResult>();
            
            var uploadFunc = taskModule.Action(nameof(TaskModulesController.Upload));
            uploadFunc.Returns<IActionResult>();

            // TaskVariants ============================================================================================
            var taskVariant = builder.EntitySet<TaskVariant>("TaskVariants").EntityType;
            taskVariant.HasKey(v => v.Id);
            taskVariant.HasRequired(v => v.TaskModule);

            taskVariant.Function(nameof(TaskVariantsController.Json)).Returns<IActionResult>();
            
            // Users ===================================================================================================
            const string usersEntitySet = "Users";
            var user = builder.EntitySet<User>(usersEntitySet).EntityType;
            user.HasKey(u => u.Id);
            user.Abstract();
            user.Ignore(u => u.PasswordHash);
            user.Ignore(u => u.PasswordSalt);

            var teacher = builder.EntitySet<Teacher>("Teachers").EntityType;
            teacher.DerivesFrom<User>();

            var student = builder.EntitySet<Student>("Students").EntityType;
            student.DerivesFrom<User>();
            student.HasMany(s => s.Logs);
            
            // TaskVariantLogs =========================================================================================
            var taskVariantLog = builder.EntitySet<TaskVariantLog>("TaskVariantLogs").EntityType;
            taskVariantLog.HasKey(l => l.Id);
            taskVariantLog.HasRequired(l => l.Student);
            taskVariantLog.HasRequired(l => l.Variant);
            
            // Unbound operations ======================================================================================
            var downloadImageFunc = builder.Function(nameof(ImagesLibraryController.DownloadImage));
            downloadImageFunc.Parameter<string>("name");
            downloadImageFunc.Returns(typeof(IActionResult));

            var currentUser = builder.Function(nameof(UsersController.CurrentUser));
            currentUser.ReturnsFromEntitySet<User>(usersEntitySet);
            
            builder.EnableLowerCamelCase();
            
            return builder.GetEdmModel();
        }
    }
}