using System.Linq;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("students")]
    public class StudentsController : ODataController
    {
        private readonly GraphLabsContext _db;

        public StudentsController(GraphLabsContext context)
        {
            _db = context;
        }
        
        [EnableQuery]
        public IQueryable<Student> Get()
        {
            return _db.Students;
        }
        
        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<Student> Get(long key)
        {
            return SingleResult.Create(_db.Students.Where(v => v.Id == key));
        }
    }
}