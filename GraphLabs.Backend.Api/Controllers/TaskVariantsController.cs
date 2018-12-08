using System;
using System.Linq;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("taskVariants")]
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
        
        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<TaskVariant> Get(long key)
        {
            return SingleResult.Create(_db.TaskVariants.Where(v => v.Id == key));
        }
    }
}