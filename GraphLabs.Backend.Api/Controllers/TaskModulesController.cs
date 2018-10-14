using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace GraphLabs.Backend.Api.Controllers
{
    public class TaskModulesController : ODataController
    {
        private readonly GraphLabsContext _db;
        private readonly IHostingEnvironment _env;
        private readonly IContentTypeProvider _contentTypeProvider;
        
        public TaskModulesController(GraphLabsContext context, 
            InMemoryInitialData initialData, 
            IHostingEnvironment env,
            IContentTypeProvider contentTypeProvider)
        {
            _db = context;
            _env = env;
            _contentTypeProvider = contentTypeProvider;
            
            if (!context.TaskModules.Any())
            {
                foreach (var module in initialData.GetTaskModules())
                {
                    context.TaskModules.Add(module);
                }
                context.SaveChanges();
            }
        }
        
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.TaskModules);
        }
        
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody]TaskModule module)
        {
            _db.TaskModules.Add(module);
            await _db.SaveChangesAsync();
            
            return Created(module);
        }

        [HttpGet]
        [ODataRoute("TaskModules({key})/Default.Download(path={path})")]
        public IActionResult Download(int key, string path)
        {
            var targetPath = Path.Combine(
                "modules",
                key.ToString(CultureInfo.InvariantCulture),
                path);
            
            var file = _env.WebRootFileProvider.GetFileInfo(targetPath);

            if (file.Exists
                && !file.IsDirectory
                && _contentTypeProvider.TryGetContentType(targetPath, out var contentType))
            {
                return File(file.CreateReadStream(), contentType);
            }
            else
            {
                return NotFound();
            }
        }
    }
}