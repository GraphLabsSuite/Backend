using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("testStudentAnswers")]
    public class TestStudentAnswersController : ODataController
    {
        private readonly GraphLabsContext _db;

        public TestStudentAnswersController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<TestStudentAnswer> Get()
        {
            return _db.TestStudentAnswers;
        }

        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<TestStudentAnswer> Get(long key)
        {
            return SingleResult.Create(_db.TestStudentAnswers.Where(t => t.Id == key));
        }
    }
}