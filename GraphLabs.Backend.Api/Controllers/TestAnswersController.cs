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
    [ODataRoutePrefix("testAnswers")]
    public class TestAnswersController : ODataController
    {
        private readonly GraphLabsContext _db;

        public TestAnswersController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<TestAnswer> Get()
        {
            return _db.TestAnswers;
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public SingleResult<TestAnswer> Get(long key)
        {
            return SingleResult.Create(_db.TestAnswers.Where(t => t.Id == key));
        }

        [HttpPost]
        [ODataRoute("({key})")]
        public async Task<IActionResult> Post([FromBody]CreateRequest request, long key)
        {
            if (request == null || string.IsNullOrEmpty(request.Answer))
                return BadRequest();

            var testQuestionVersion = _db.TestQuestionVersions.Single(v => v.Id == key);
            if (testQuestionVersion == null)
                return BadRequest();

            var testAnswer = new TestAnswer
            {
                Answer = request.Answer,
                IsRight = request.IsRight,
                TestQuestionVersion = testQuestionVersion
            };
            
            if (testQuestionVersion.TestAnswers == null)
            {
                testQuestionVersion.TestAnswers = new List<TestAnswer> { testAnswer };
            }
            else
            {
                testQuestionVersion.TestAnswers.Add(testAnswer);
            }

            await _db.SaveChangesAsync();
            return Ok(testAnswer.Id);
        }

        [HttpDelete]
        [ODataRoute("({key})")]
        public async Task<IActionResult> Delete(long key)
        {
            var answer = _db.TestAnswers.Single(v => v.Id == key);
            if (answer != null)
            {
                _db.TestAnswers.Remove(answer);
                await _db.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        public class CreateRequest
        {
            public string Answer { get; set; }

            public bool IsRight { get; set; }
        }
    }
}