using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.Api.Infrastructure;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("taskModules")]
    public class TaskModulesController : ODataController
    {
        private readonly GraphLabsContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly ModuleStore _moduleStore;

        public TaskModulesController(GraphLabsContext context,
            IHostingEnvironment hostingEnvironment,
            IContentTypeProvider contentTypeProvider,
            ModuleStore moduleStore)
        {
            _db = context;
            _hostingEnvironment = hostingEnvironment;
            _contentTypeProvider = contentTypeProvider;
            _moduleStore = moduleStore;
        }

        [EnableQuery]
        public IQueryable<TaskModule> Get()
        {
            return _db.TaskModules;
        }

        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<TaskModule> Get(long key)
        {
            return SingleResult.Create(_db.TaskModules.Where(t => t.Id == key));
        }
        
        public async Task<IActionResult> RandomVariant(long key)
        {
            var variants = await _db.TaskVariants
                .Where(variant => variant.TaskModule.Id == key)
                .Select(variant => variant.Id)
                .ToArrayAsync();


            if (variants.Length == 0)
            {
                return new NotFoundResult();
            }
            
            var variantId = variants[new Random().Next(0, variants.Length)];
            var selectedVariant = await _db.TaskVariants.SingleAsync(v => v.Id == variantId);

            var result = new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json",
                Content = $@"{{""data"": {selectedVariant.VariantData}, ""meta"": {{ ""name"": ""{selectedVariant.Name}"", ""id"": ""{selectedVariant.Id}"" }} }}"
            };

            return result;
        }


        
        public IActionResult Download(int key, [FromODataUri]string path)
        {
            var (file, contentType) = _moduleStore.Download(key, path);

            if (file != null)
            {
                return File(file, contentType);
            }
            else
            {
                return NotFound();
            }
        }
        
        public IActionResult Upload(int key)
        {
            _moduleStore.Upload(key, Request.Body);
            return Ok();
        }
    }
}