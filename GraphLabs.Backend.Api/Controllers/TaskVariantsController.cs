using System;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphLabs.Backend.Api.Controllers
{
    public class TaskVariantsController : ODataController
    {
        private readonly GraphLabsContext _db;
        
        public TaskVariantsController(GraphLabsContext context, InMemoryInitialData initialData) 
        {
            _db = context;
            
            if (!context.TaskModules.Any())
            {
                foreach (var module in initialData.GetTaskModules())
                {
                    context.TaskModules.Add(module);
                }
            }
            if (!context.TaskVariants.Any())
            {
                foreach (var variant in initialData.GetTaskVariants(context.TaskModules))
                {
                    context.TaskVariants.Add(variant);
                }
                context.SaveChanges();
            }
        }
        
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.TaskVariants);
        }
        
        [HttpGet]
        [ODataRoute("GetRandomVariant(taskId={taskId})")]
        public async Task<string> GetRandomVariant(long taskId)
        {
            var variants = await _db.TaskVariants
                .Where(variant => variant.TaskModule.Id == taskId)
                .Select(variant => variant.Id)
                .ToArrayAsync();

            if (variants.Length == 0)
                return string.Empty;

            var id = variants[new Random().Next(0, variants.Length)];
            var selectedVariant = await _db.TaskVariants.SingleAsync(v => v.Id == id);
            return $@"{{""data"": {selectedVariant.VariantData}, ""meta"": {{ ""name"": ""{selectedVariant.Name}"", id: ""{selectedVariant.Id}"" }} }}";
        }
    }
}