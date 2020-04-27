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
    public class TestStudentAnswersController : Controller
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

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var json = await Request.GetBodyAsString();
            var jsonData = TryExecute(() => JObject.Parse(json), "Не удалось распарсить данные.");
            var key = TryExecute(() => jsonData["key"].Value<long>(), "Не удалось прочитать значение key");
            var testQuestionVersion = TryExecute(() => _db.TestQuestionVersions.Single(v => v.Id == key), $"Не существует вопроса с ключом {key}");

            var answer = TryExecute(() => jsonData["answer"].Value<string>(), "Не удалось прочитать значение TestStudentAnswer answer");
            if (answer == null || answer == "")
            {
                throw new TestStudentAnswerConvertException("Полученное значение answer пусто");
            }

            var answerId = TryExecute(() => jsonData["answer_id"].Value<long>(), "Не удалось прочитать значение TestStudentAnswer answer_id");
            var checkAnswerId = TryExecute(() => _db.TestAnswers.Single(v => v.Id == answerId), $"Не существует ответа с answer_id {answerId}");

            var isRight = TryExecute(() => jsonData["is_right"].Value<bool>(), "Не удалось прочитать значение TestStudentAnswer is_right");

            var testStudentAnswer = new TestStudentAnswer
            {
                Answer = answer,
                AnswerId = answerId,
                IsRight = isRight,
                TestQuestionVersion = testQuestionVersion
            };
            _db.TestStudentAnswers.Add(testStudentAnswer);

            var collection = testQuestionVersion.TestStudentAnswers;
            collection.Add(testStudentAnswer);

            testQuestionVersion.TestStudentAnswers = collection;

            // сделать добавление в таблицу TestResults, а значит нужно еще 
            // написать метод проверки ответов

            await _db.SaveChangesAsync();
            return Ok(key);
        }

        private class TestStudentAnswerConvertException : Exception
        {
            public TestStudentAnswerConvertException(string error) : base(error)
            {
            }

            public TestStudentAnswerConvertException(string error, Exception inner) : base(error, inner)
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
                throw new TestStudentAnswerConvertException(errorMessage, e);
            }
        }
    }
}