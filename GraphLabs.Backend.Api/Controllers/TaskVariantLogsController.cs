using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _contextAccessor;

        public TaskVariantLogsController(GraphLabsContext context,
            IHttpContextAccessor contextAccessor)
        {
            _db = context;
            _contextAccessor = contextAccessor;
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
        public async Task<IActionResult> Post([FromBody]CreateLogRequest request)
        {
            if (request == null ||
                request.VariantId == 0 ||
                string.IsNullOrEmpty(request.Action))
            {
                return PreconditionFailed();
            }

            var email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var student = await _db.Students.SingleOrDefaultAsync(s => s.Email == email);
            if (student == null)
                return BadRequest();
            
            var logEntry = new TaskVariantLog
            {
                Action = request.Action,
                DateTime = DateTime.Now,
                StudentId = student.Id,
                VariantId = request.VariantId
            };

            await _db.TaskVariantLogs.AddAsync(logEntry);
            await _db.SaveChangesAsync();

            return Created(logEntry);
        }

        public class CreateLogRequest
        {
            public string Action { get; set; }
        
            public long VariantId { get; set; }
        }

        
        private StatusCodeResult PreconditionFailed()
        {
            return new StatusCodeResult(StatusCodes.Status412PreconditionFailed);
        }
            
    }
}