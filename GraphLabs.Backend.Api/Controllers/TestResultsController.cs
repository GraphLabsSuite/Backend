using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("testResults")]
    public class TestResultsController : Controller
    {
        private readonly GraphLabsContext _db;

        public TestResultsController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<TestResult> Get()
        {
            return _db.TestResults;
        }

        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<TestResult> Get(long key)
        {
            return SingleResult.Create(_db.TestResults.Where(t => t.Id == key));
        }
    }
}