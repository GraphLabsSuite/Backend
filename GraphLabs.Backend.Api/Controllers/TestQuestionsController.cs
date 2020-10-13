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
    [ODataRoutePrefix("testQuestions")]
    public class TestQuestionsController : ODataController
    {
        private readonly GraphLabsContext _db;

        public TestQuestionsController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<TestQuestion> Get()
        {
            return _db.TestQuestions;
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        public SingleResult<TestQuestion> Get(long key)
        {
            return SingleResult.Create(_db.TestQuestions.Where(t => t.Id == key));
        }

        [HttpPost]
        [ODataRoute("({key})")]
        public async Task<IActionResult> Post([FromBody]CreateRequest request, long key)
        {
            if (request == null ||
                request.MaxScore == 0 ||
                string.IsNullOrEmpty(request.PlainText) ||
                string.IsNullOrEmpty(request.Difficulty))
                return BadRequest();

            TestQuestion testQuestion;
            var testQuestionVersion = new TestQuestionVersion
            {
                PlainText = request.PlainText,
                Difficulty = GetDifficulty(request.Difficulty),
                MaxScore = request.MaxScore
            };

            if (key == 0)
            {
                //Create TestQuestion and TestQuestion.TestQuestionVersion
                var subject = _db.Subjects.Single(s => s.Id == request.SubjectId);
                if (subject == null)
                    return BadRequest();

                testQuestion = new TestQuestion
                {
                    TestQuestionVersions = new List<TestQuestionVersion> { testQuestionVersion },
                    Subject = subject
                };

                if (subject.TestQuestions == null)
                {
                    subject.TestQuestions = new List<TestQuestion> { testQuestion };
                }
                else
                {
                    subject.TestQuestions.Add(testQuestion);
                }
                _db.TestQuestions.Add(testQuestion);
            }
            else
            {
                //Update TestQuestion.TestQuestionVersion
                testQuestion = _db.TestQuestions.Include(q => q.TestQuestionVersions).Single(q => q.Id == key);
                testQuestion.TestQuestionVersions.Add(testQuestionVersion);
            }

            testQuestionVersion.TestQuestion = testQuestion;
            _db.TestQuestionVersions.Add(testQuestionVersion);

            await _db.SaveChangesAsync();
            return Ok(testQuestionVersion.Id);
        }

        public class CreateRequest
        {
            public string PlainText { get; set; }

            public string Difficulty { get; set; }

            public int MaxScore { get; set; }

            public long? SubjectId { get; set; }
        }

        private Difficulty GetDifficulty(string diff)
        {
            switch (diff)
            {
                case "Three":
                    return Difficulty.Three;
                case "Four":
                    return Difficulty.Four;
                case "Five":
                    return Difficulty.Five;
            }
            return Difficulty.Three;
        }
    }
}