using System;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace GraphLabs.Backend.Api.Controllers
{
    public class TaskModulesController : ODataController
    {
        private readonly GraphLabsContext _db;
        
        public TaskModulesController(GraphLabsContext context, InMemoryInitialData initialData) 
        {
            _db = context;
            
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
    }
}