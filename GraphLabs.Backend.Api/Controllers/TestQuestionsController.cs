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
    [ODataRoutePrefix("testQuestions")]
    public class TestQuestionsController : Controller
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

        
        // не работает route, что странно, в TaskVariantsLogsController это есть
        // передаю TestQuestion.Id(то есть key) в json
        // если key == null, то создается новый вопрос, иначе дополняется в ветку версий вопросов
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            //Проверка полученных данных
            var json = await Request.GetBodyAsString();
            var jsonData = TryExecute(() => JObject.Parse(json), "Не удалось распарсить данные.");
            var key = jsonData["key"];

            var plainText = TryExecute(() => jsonData["plain_text"].Value<string>(), "Не удалось прочитать значение TestQuestion plain_text");
            if (plainText == null || plainText == "")
            {
                throw new TestQuestionConvertException("Полученное значение plain_text пусто");
            }

            var difficulty = new Difficulty();
            var difficultyString = TryExecute(() => jsonData["difficulty"].Value<string>(), "Не удалось прочитать значение TestQuestion difficulty");
            if (difficultyString == null || difficultyString == "")
            {
                throw new TestQuestionConvertException("Полученное значение difficulty пусто");
            }
            // костыль start
            switch (difficultyString)
            {
                case "Three":
                    difficulty = Difficulty.Three;
                    break;
                case "Four":
                    difficulty = Difficulty.Four;
                    break;
                case "Five":
                    difficulty = Difficulty.Five;
                    break;
                default:
                    throw new TestQuestionConvertException("Полученное значение difficulty не соответсвует требованиям");
            }
            // костыль end    

            var maxScore = TryExecute(() => jsonData["max_score"].Value<int>(), "Не удалось прочитать значение TestQuestion max_score");
            if (maxScore.Equals(null))
            {
                throw new TestQuestionConvertException("Полученное значение max_score пусто");
            }

            if (key.Type != JTokenType.Null)
            {
                //Update
                var testQuestion = _db.TestQuestions.Single(q => q.Id == (long)key);
                if (testQuestion == null)
                {
                    throw new TestQuestionConvertException("Идентификатор key не совпадает ни с одним TestQuestion.Id");
                }

                var testQuestionVersion = new TestQuestionVersion
                {
                    PlainText = plainText,
                    Difficulty = difficulty,
                    MaxScore = maxScore,
                    TestQuestion = testQuestion
                };

                _db.TestQuestionVersions.Add(testQuestionVersion);    
            }
            else
            {
                //Create
                var idSubject = TryExecute(() => jsonData["subject"].Value<long>(), "Не удалось прочитать значение TestQuestion subject");
                if (idSubject.Equals(null))
                {
                    throw new TestQuestionConvertException("Полученное значение subject пусто");
                }
                
                var testQuestion = new TestQuestion();
                testQuestion.Subject = _db.Subjects.Single(s => s.Id == idSubject);

                var testQuestionVersion = new TestQuestionVersion
                {
                    PlainText = plainText,
                    Difficulty = difficulty,
                    MaxScore = maxScore,
                    TestQuestion = testQuestion
                };

                var collection = new Collection<TestQuestionVersion>();
                collection.Add(testQuestionVersion);

                testQuestion.TestQuestionVersions = collection;

                _db.TestQuestionVersions.Add(testQuestionVersion);
                _db.TestQuestions.Add(testQuestion);
            }
            // сделать добавление в таблицу Subjects
            await _db.SaveChangesAsync();
            return Ok();
        }

        private class TestQuestionConvertException : Exception
        {
            public TestQuestionConvertException(string error) : base(error)
            {
            }

            public TestQuestionConvertException(string error, Exception inner) : base(error, inner)
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
                throw new TestQuestionConvertException(errorMessage, e);
            }
        }
    }
}