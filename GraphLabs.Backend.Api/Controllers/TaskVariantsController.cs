using System;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphLabs.Backend.Api.Controllers
{
    public class TaskVariantsController : ODataController
    {
        private readonly GraphLabsContext _db;

        public TaskVariantsController(GraphLabsContext context)
        {
            _db = context;
        }
        
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.TaskVariants);
        }
        
        [HttpGet]
        [ODataRoute("GetRandomVariant(taskId={taskId})")]
        public async Task<IActionResult> GetRandomVariant(long taskId)
        {
            var variants = await _db.TaskVariants
                .Where(variant => variant.TaskModule.Id == taskId)
                .Select(variant => variant.Id)
                .ToArrayAsync();


            if (variants.Length == 0)
            {
                return new NotFoundResult();
            }
            
            var id = variants[new Random().Next(0, variants.Length)];
            var selectedVariant = await _db.TaskVariants.SingleAsync(v => v.Id == id);

            var result = new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json",
                Content = $@"{{""data"": {selectedVariant.VariantData}, ""meta"": {{ ""name"": ""{selectedVariant.Name}"", ""id"": ""{selectedVariant.Id}"" }} }}"
            };

            return result;
        }
    }
}