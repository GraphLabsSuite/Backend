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

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("testResults")]
    public class TestResultsController : ODataController
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRequest[] request)
        {
            if (request == null)
                return BadRequest();

            var testResult = new TestResult();
            float fullScore = 0;
            float studentScore = 0;
            var collectionStudentAnswers = new Collection<TestStudentAnswer>();
            foreach (var r in request)
            {
                if (r == null ||
                    r.AnswerId == 0 ||
                    r.StudentId == 0 ||
                    r.TestQuestionVersionId == 0 ||
                    string.IsNullOrEmpty(r.Answer))
                {
                    return BadRequest();
                }

                var testQuestionVersion = _db.TestQuestionVersions.Single(v => v.Id == r.TestQuestionVersionId);
                var testStudentAnswer = new TestStudentAnswer
                {
                    AnswerId = r.AnswerId,
                    Answer = r.Answer,
                    IsRight = r.IsRight,
                    TestQuestionVersion = testQuestionVersion,
                    TestResult = testResult
                };
                var testAnswer = _db.TestAnswers.Single(p => p.Id == r.AnswerId);
                float maxScoreForTask = (float) testStudentAnswer.TestQuestionVersion.MaxScore;
                float maxScoreForOneQuestion = (float) (maxScoreForTask / _db.TestAnswers.Count(t => t.TestQuestionVersion.Id == testStudentAnswer.TestQuestionVersion.Id));
                if (testStudentAnswer.Answer == testAnswer.Answer && testStudentAnswer.IsRight == testAnswer.IsRight)
                    studentScore += maxScoreForOneQuestion;
                fullScore += maxScoreForOneQuestion;

                if (testQuestionVersion.TestStudentAnswers == null)
                {
                    testQuestionVersion.TestStudentAnswers = new List<TestStudentAnswer> { testStudentAnswer };
                }
                else
                {
                    testQuestionVersion.TestStudentAnswers.Add(testStudentAnswer);
                }
                collectionStudentAnswers.Add(testStudentAnswer);
            }

            var student = _db.Students.Single(s => s.Id == request[0].StudentId);
            int score = (int)((100 * studentScore) / fullScore);
            testResult.Score = score;
            testResult.DateTime = DateTime.Now;
            testResult.MarkEU = GetMarkEU(score);
            testResult.MarkRU = GetMarkRU(score);
            testResult.TestStudentAnswer = collectionStudentAnswers;
            testResult.Student = student;

            if (student.TestResults == null)
            {
                student.TestResults = new List<TestResult> { testResult };
            }
            else
            {
                student.TestResults.Add(testResult);
            }

            //await _db.SaveChangesAsync();
            return Created(testResult.Id);
        }

        public class CreateRequest
        {
            public long TestQuestionVersionId { get; set; }

            public string Answer { get; set; }

            public long AnswerId { get; set; }

            public bool IsRight { get; set; }

            public long StudentId { get; set; }
        }

        private MarkRU GetMarkRU(int score)
        {
            if (score >= 0 && score <= 59)
                return MarkRU.Two;
            if (score >= 60 && score <= 69)
                return MarkRU.Three;
            if (score >= 70 && score <= 89)
                return MarkRU.Four;
            if (score >= 90 && score <= 100)
                return MarkRU.Five;
            return MarkRU.Two;
        }

        private MarkEU GetMarkEU(int score)
        {
            if (score >= 0 && score <= 59)
                return MarkEU.F;
            if (score >= 60 && score <= 64)
                return MarkEU.E;
            if (score >= 65 && score <= 74)
                return MarkEU.D;
            if (score >= 75 && score <= 84)
                return MarkEU.C;
            if (score >= 85 && score <= 89)
                return MarkEU.B;
            if (score >= 90 && score <= 100)
                return MarkEU.A;
            return MarkEU.F;
        }
    }
}