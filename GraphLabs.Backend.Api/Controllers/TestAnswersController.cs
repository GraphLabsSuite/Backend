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
using Newtonsoft.Json.Linq;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("testAnswers")]
    public class TestAnswersController : Controller
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
        public async Task<IActionResult> Post()
        {
            var json = await Request.GetBodyAsString();
            var jsonData = TryExecute(() => JObject.Parse(json), "Не удалось распарсить данные.");
            var key = TryExecute(() => jsonData["key"].Value<long>(), "Не удалось прочитать значение key");
            var testQuestionVersion = TryExecute(() => _db.TestQuestionVersions.Single(v => v.Id == key), $"Не существует вопроса с ключом {key}");
            
            var answer = TryExecute(() => jsonData["answer"].Value<string>(), "Не удалось прочитать значение TestAnswer answer");
            if (answer == null || answer == "")
            {
                throw new TestAnswerConvertException("Полученное значение answer пусто");
            }

            var isRight = TryExecute(() => jsonData["is_right"].Value<bool>(), "Не удалось прочитать значение TestAnswer is_right");
            
            var testAnswer = new TestAnswer
            {
                Answer = answer,
                IsRight = isRight,
                TestQuestionVersion = testQuestionVersion
            };
            _db.TestAnswers.Add(testAnswer);

            var collection = testQuestionVersion.TestAnswers;
            collection.Add(testAnswer);

            testQuestionVersion.TestAnswers = collection;

            await _db.SaveChangesAsync();
            return Ok(key);
        }

        private class TestAnswerConvertException : Exception
        {
            public TestAnswerConvertException(string error) : base(error)
            {
            }

            public TestAnswerConvertException(string error, Exception inner) : base(error, inner)
            {
            }
        }

        private static T TryExecute<T>(Func<T> f, string errorMessage)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                throw new TestAnswerConvertException(errorMessage, e);
            }
        }

    }
}