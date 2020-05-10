using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("subjects")]
    public class SubjectsController : ODataController
    {
        private readonly GraphLabsContext _db;

        public SubjectsController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<Subject> Get()
        {
            return _db.Subjects;
        }

        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<Subject> Get(long key)
        {
            return SingleResult.Create(_db.Subjects.Where(t => t.Id == key));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Name) ||
                string.IsNullOrEmpty(request.Description))
                return BadRequest();

            var subject = new Subject
            {
                Name = request.Name,
                Description = request.Description
            };

            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();

            return Ok(subject.Id);
        }

        public class CreateRequest
        {
            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}