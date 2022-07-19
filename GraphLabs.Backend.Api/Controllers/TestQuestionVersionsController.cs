using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("testQuestionVersions")]
    public class TestQuestionVersionsController : ODataController
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


        //ODataRoute("createVariant") not working
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRequest[] request)
        {
            if (request == null)
                return BadRequest();

            var variant = new Collection<TestQuestionVersion>();

            foreach (var r in request)
            {
                if (r.SubjectId == 0 || r.Quantity == 0)
                    return BadRequest();

                var subjectId = r.SubjectId;
                var quantity = r.Quantity;

                var question = _db.TestQuestions.Where(w => w.Subject.Id == subjectId);
                var questionLength = question.Count();

                if (quantity > questionLength)
                    quantity = questionLength;
                if (questionLength == 0)
                    return new NotFoundResult();

                var questionArray = question.Select(s => s.Id).ToArray();
                for (var i = 0; i<quantity; i++)
                {
                    var questionArrayRandomId = questionArray[new Random().Next(0, questionArray.Length)];
                    questionArray = questionArray.Where(w => w != questionArrayRandomId).ToArray();
                    var questionVersion = _db.TestQuestionVersions.Last(p => p.TestQuestion.Id == questionArrayRandomId);
                    variant.Add(questionVersion);
                }
            }

            return Ok(variant);
        }

        public class CreateRequest
        {
            public long SubjectId { get; set; }

            public int Quantity { get; set; }
        }

    }
}