using System;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("taskVariantLogs")]
    public class TaskVariantLogsController : ODataController
    {
        private readonly GraphLabsContext _db;

        public TaskVariantLogsController(GraphLabsContext context)
        {
            _db = context;
        }
        
        [EnableQuery]
        public IQueryable<TaskVariantLog> Get()
        {
            return _db.TaskVariantLogs;
        }
        
        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<TaskVariantLog> Get(long key)
        {
            return SingleResult.Create(_db.TaskVariantLogs.Where(v => v.Id == key));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateLogDto dto)
        {
            if (dto == null ||
                dto.StudentId == 0 ||
                dto.VariantId == 0 ||
                string.IsNullOrEmpty(dto.Action))
            {
                return PreconditionFailed();
            }
            
            var logEntry = new TaskVariantLog
            {
                Action = dto.Action,
                DateTime = DateTime.Now,
                StudentId = dto.StudentId,
                VariantId = dto.VariantId
            };

            await _db.TaskVariantLogs.AddAsync(logEntry);
            await _db.SaveChangesAsync();

            return Created(logEntry);
        }

        public class CreateLogDto
        {
            public string Action { get; set; }
        
            public long VariantId { get; set; }

            public long StudentId { get; set; }
        }

        
        private StatusCodeResult PreconditionFailed()
        {
            return new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
        }
            
    }
}