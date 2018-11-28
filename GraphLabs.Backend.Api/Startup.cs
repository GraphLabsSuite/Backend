using System;
using System.IO;
using GraphLabs.Backend.Api.Controllers;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                services.AddDbContext<GraphLabsContext>(options => options.UseInMemoryDatabase("GraphLabs"));
                services.AddSingleton<InMemoryInitialData>();
                services.AddCors();
            }
            else
            {
                throw new NotImplementedException("Для продакшна сделаем чуть позже");
            }

            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddOData();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app/*, IHostingEnvironment env*/)
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

            app.UseMvc(builder =>
            {
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                builder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }
        
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "GraphLabs";
            
            builder.EntitySet<TaskModule>("TaskModules");
            builder.EntitySet<TaskVariant>("TaskVariants");

            var downloadModuleFunc = builder.EntityType<TaskModule>()
                .Function(nameof(TaskModulesDownloadController.Download));
            downloadModuleFunc.Parameter<string>("path");
            downloadModuleFunc.Returns(typeof(IActionResult));

            var downloadImageFunc = builder.Function(nameof(ImagesLibraryController.DownloadImage));
            downloadImageFunc.Parameter<string>("name");
            downloadImageFunc.Returns(typeof(IActionResult));
            
            var getVariantFunc = builder.Function(nameof(TaskVariantsController.GetRandomVariant));
            getVariantFunc.Parameter<long>("taskId");
            getVariantFunc.Returns(typeof(IActionResult));

            return builder.GetEdmModel();
        }
    }
}