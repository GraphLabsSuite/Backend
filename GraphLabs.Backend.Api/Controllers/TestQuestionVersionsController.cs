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
    [ODataRoutePrefix("testQuestionVersions")]
    public class TestQuestionVersionsController : Controller
    {
        private readonly GraphLabsContext _db;

        public TestQuestionVersionsController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<TestQuestionVersion> Get()
        {
            return _db.TestQuestionVersions;
        }

        [EnableQuery]
        [ODataRoute("({key})")] 
        public SingleResult<TestQuestionVersion> Get(long key)
        {
            return SingleResult.Create(_db.TestQuestionVersions.Where(t => t.Id == key));
        }
    }
}